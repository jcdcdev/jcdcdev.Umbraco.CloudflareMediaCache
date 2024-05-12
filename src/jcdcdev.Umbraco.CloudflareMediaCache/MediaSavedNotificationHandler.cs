using jcdcdev.Umbraco.CloudflareMediaCache.Api;
using jcdcdev.Umbraco.CloudflareMediaCache.Api.Models;
using jcdcdev.Umbraco.CloudflareMediaCache.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

namespace jcdcdev.Umbraco.CloudflareMediaCache;

public class MediaSavedNotificationHandler : INotificationAsyncHandler<MediaSavedNotification>
{
    private readonly ILogger _logger;
    private readonly CloudflareCacheOptions _options;
    private readonly IUmbracoContextFactory _umbracoContextFactory;
    private readonly IPublishedUrlProvider _publishedUrlProvider;
    private readonly ICloudflareCacheApiClient _cloudflareCacheApiClient;

    public MediaSavedNotificationHandler(IOptions<CloudflareCacheOptions> options,
        IUmbracoContextFactory umbracoContextFactory,
        IPublishedUrlProvider publishedUrlProvider,
        ICloudflareCacheApiClient cloudflareCacheApiClient,
        ILogger<MediaSavedNotificationHandler> logger)
    {
        _umbracoContextFactory = umbracoContextFactory;
        _publishedUrlProvider = publishedUrlProvider;
        _cloudflareCacheApiClient = cloudflareCacheApiClient;
        _logger = logger;
        _options = options.Value;
    }

    public async Task HandleAsync(MediaSavedNotification notification, CancellationToken cancellationToken)
    {
        if (!_options.Enabled)
        {
            _logger.LogInformation("Cloudflare cache purging is disabled");
            return;
        }

        if (_options.Mode == PurgeCacheMode.All)
        {
            _logger.LogInformation("Purging all Cloudflare cache");
            var result = await _cloudflareCacheApiClient.SendPurgeAllRequest();
            if (!result)
            {
                _logger.LogError("Failed to purge Cloudflare cache");
            }
            else
            {
                _logger.LogInformation("Successfully purged Cloudflare cache");
            }

            return;
        }

        using var ctx = _umbracoContextFactory.EnsureUmbracoContext();
        var mediaCache = ctx.UmbracoContext.Media;
        if (mediaCache == null)
        {
            _logger.LogError("Failed to get published media cache");
            return;
        }

        var publishedMedia = new List<IPublishedContent>();
        foreach (var saved in notification.SavedEntities)
        {
            var media = mediaCache.GetById(saved.Id);
            if (media == null)
            {
                _logger.LogWarning("Failed to get published media with id {Id}", saved.Id);
                continue;
            }

            publishedMedia.Add(media);
        }

        var request = _options.Mode switch
        {
            PurgeCacheMode.Prefix => PreparePurgeByPrefixRequest(publishedMedia),
            _ => throw new ArgumentOutOfRangeException()
        };

        _logger.LogInformation("Purging Cloudflare cache {Mode}", _options.Mode);
        await _cloudflareCacheApiClient.SendPurgeRequest(request);
    }

    private PurgeCacheRequest PreparePurgeByPrefixRequest(List<IPublishedContent> publishedMedia)
    {
        var request = new PurgeCacheRequest
        {
            Prefixes = new List<string>()
        };

        foreach (var media in publishedMedia)
        {
            foreach (var mediaCulture in media.Cultures)
            {
                var url = media.MediaUrl(_publishedUrlProvider, mediaCulture.Key, UrlMode.Absolute);
                var uri = new UriBuilder(url);
                uri.Path = string.Join("/", uri.Path.Split("/").SkipLast());
                request.Prefixes.Add(uri.ToString());
            }
        }

        return request;
    }
}
