using System.Text.Json.Serialization;

namespace jcdcdev.Umbraco.CloudflareMediaCache.Api.Models;

public class Owner
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("type")] public string Type { get; set; }

    [JsonPropertyName("email")] public string Email { get; set; }
}
