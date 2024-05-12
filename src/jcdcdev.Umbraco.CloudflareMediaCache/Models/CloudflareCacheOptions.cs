namespace jcdcdev.Umbraco.CloudflareMediaCache.Models;

public class CloudflareCacheOptions
{
    public readonly Uri BaseUrl = new("https://api.cloudflare.com/client/v4/");
    public string? ApiToken { get; set; }
    public string? ZoneId { get; set; }
    public bool Enabled { get; set; }
    public PurgeCacheMode Mode { get; set; }
    public int MaxAge { get; set; } = 3600;
    public TimeSpan? MaxAgeTimeSpan => TimeSpan.FromSeconds(MaxAge);
}
