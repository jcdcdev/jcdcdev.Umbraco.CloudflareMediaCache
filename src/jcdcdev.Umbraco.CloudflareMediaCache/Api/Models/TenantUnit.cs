using System.Text.Json.Serialization;

namespace jcdcdev.Umbraco.CloudflareMediaCache.Api.Models;

public class TenantUnit
{
    [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;
}
