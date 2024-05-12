using System.Net.Http.Json;
using System.Text.Json;
using jcdcdev.Umbraco.CloudflareMediaCache.Api.Models;
using jcdcdev.Umbraco.CloudflareMediaCache.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core;

namespace jcdcdev.Umbraco.CloudflareMediaCache.Api;

public class CloudflareCacheApiClient : ICloudflareCacheApiClient
{
    private readonly HttpClient _httpClient;

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly ILogger _logger;
    private readonly CloudflareCacheOptions _options;

    public CloudflareCacheApiClient(HttpClient httpClient, IOptions<CloudflareCacheOptions> options, ILogger<CloudflareCacheApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _options = options.Value;
    }

    public async Task<bool> SendPurgeRequest(PurgeCacheRequest request) => await PurgeInternal(request);

    public async Task<bool> SendPurgeAllRequest() => await PurgeInternal(new PurgeCacheRequest
    {
        PurgeEverything = true
    });

    public async Task<Attempt<ZoneResponse?>> GetZoneDetails()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ZoneResponse>($"zones/{_options.ZoneId}");

            return Attempt.If(response?.Success ?? false, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get zone details");
            return Attempt<ZoneResponse?>.Fail(ex);
        }
    }

    private async Task<bool> PurgeInternal(PurgeCacheRequest? request = null)
    {
        var url = $"zones/{_options.ZoneId}/purge_cache";
        _logger.LogDebug("Purging Cloudflare cache {Url}", url);
        var response = await _httpClient.PostAsJsonAsync(url, request, _jsonSerializerOptions);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to purge Cloudflare cache {StatusCode}", response.StatusCode);
            return false;
        }

        var purgeResponse = await response.Content.ReadFromJsonAsync<PurgeCacheResponse>();
        _logger.LogDebug("{Body}", purgeResponse);
        return purgeResponse?.Success ?? false;
    }
}
