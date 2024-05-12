using System.Text.Json.Serialization;

namespace jcdcdev.Umbraco.CloudflareMediaCache.Api.Models;

public class ZoneResponse
{
    [JsonPropertyName("result")] public Result Result { get; set; }

    [JsonPropertyName("success")] public bool Success { get; set; }

    [JsonPropertyName("errors")] public List<string> Errors { get; set; }

    [JsonPropertyName("messages")] public List<string> Messages { get; set; }
}
