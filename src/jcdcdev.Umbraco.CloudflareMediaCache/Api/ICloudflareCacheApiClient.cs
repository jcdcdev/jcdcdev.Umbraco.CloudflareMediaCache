using jcdcdev.Umbraco.CloudflareMediaCache.Api.Models;
using Umbraco.Cms.Core;

namespace jcdcdev.Umbraco.CloudflareMediaCache.Api;

public interface ICloudflareCacheApiClient
{
    Task<bool> SendPurgeRequest(PurgeCacheRequest request);
    Task<bool> SendPurgeAllRequest();
    Task<Attempt<ZoneResponse?>> GetZoneDetails();
}
