using System.Text.Json.Serialization;

namespace jcdcdev.Umbraco.CloudflareMediaCache.Api.Models;

public class PurgeCacheRequest
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Uri>? Files { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? Prefixes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? Tags { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("purge_everything")]
    public bool PurgeEverything { get; set; }
}
