using System.Security.Cryptography;
using System.Text;
using jcdcdev.Umbraco.CloudflareMediaCache.Api;
using jcdcdev.Umbraco.CloudflareMediaCache.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using SixLabors.ImageSharp.Web.Middleware;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace jcdcdev.Umbraco.CloudflareMediaCache.Composing;

public static class UmbracoBuilderExtensions
{
    public static void AddCloudflareMediaCache(this IUmbracoBuilder builder)
    {
        builder.HealthChecks().Add<CloudflareCacheHealthCheck>();
        builder.Services.AddHttpClient<ICloudflareCacheApiClient, CloudflareCacheApiClient>((services, client) =>
        {
            var options = services.GetRequiredService<IOptions<CloudflareCacheOptions>>().Value;
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + options.Key);
            client.BaseAddress = options.BaseUrl;
        });

        builder.Services
            .AddOptions<CloudflareCacheOptions>()
            .BindConfiguration("Cloudflare:Media:Cache")
            .Configure<IConfiguration>((x, config) =>
            {
                x.ZoneId = x.ZoneId.IfNullOrWhiteSpace(config.GetSection("Cloudflare:ZoneId").Value) ??
                           throw new Exception("ZoneId cannot be determined");
            });

        builder.AddNotificationAsyncHandler<MediaSavedNotification, MediaSavedNotificationHandler>();

        builder.Services.Configure<ImageSharpMiddlewareOptions>(options =>
        {
            options.OnPrepareResponseAsync = OnPrepareResponseAsync;
        });
    }

    private static Task OnPrepareResponseAsync(HttpContext context)
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<object>>();
        try
        {
            var value = context.RequestServices.GetRequiredService<IOptions<CloudflareCacheOptions>>().Value;
            if (!value.Enabled)
            {
                logger.LogDebug("Cloudflare media caching is disabled");
                return Task.CompletedTask;
            }

            logger.LogDebug("Writing Cloudflare media cache headers");
            var headers = context.Response.GetTypedHeaders();
            var cacheControl = headers.CacheControl ?? new CacheControlHeaderValue();
            cacheControl.Public = true;
            cacheControl.MaxAge = value.MaxAgeTimeSpan;
            headers.CacheControl = cacheControl;

            var mediaService = context.RequestServices.GetRequiredService<IMediaService>();
            var media = mediaService.GetMediaByPath(context.Request.Path);
            if (media == null)
            {
                logger.LogWarning("Media not found for path {Path}", context.Request.Path);
                return Task.CompletedTask;
            }

            var key = CreateKey(context, media);
            headers.ETag = new EntityTagHeaderValue($"\"{key}\"");
            logger.LogDebug("media {Media} ETag set to {ETag}", media.Id, headers.ETag);
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error setting Cloudflare media cache headers");
            return Task.CompletedTask;
        }
    }

    private static string CreateKey(HttpContext context, IMedia media)
    {
        var src =
            $"{media.UpdateDate.Ticks}{context.Request.Path}{context.Request.Query.OrderBy(x => x.Key).Select(x => $"{x.Key}{x.Value}")}";
        var key = BitConverter
            .ToString(MD5.HashData(Encoding.UTF8.GetBytes(src)))
            .Replace("-", "");
        return key;
    }
}