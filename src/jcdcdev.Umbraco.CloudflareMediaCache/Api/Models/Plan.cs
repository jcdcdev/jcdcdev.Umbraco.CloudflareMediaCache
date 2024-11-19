using System.Text.Json.Serialization;

namespace jcdcdev.Umbraco.CloudflareMediaCache.Api.Models;

public class Plan
{
    [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;

    [JsonPropertyName("price")] public int Price { get; set; }

    [JsonPropertyName("currency")] public string Currency { get; set; } = string.Empty;

    [JsonPropertyName("frequency")] public string Frequency { get; set; } = string.Empty;

    [JsonPropertyName("is_subscribed")] public bool IsSubscribed { get; set; }

    [JsonPropertyName("can_subscribe")] public bool CanSubscribe { get; set; }

    [JsonPropertyName("legacy_id")] public string LegacyId { get; set; } = string.Empty;

    [JsonPropertyName("legacy_discount")] public bool LegacyDiscount { get; set; }

    [JsonPropertyName("externally_managed")]
    public bool ExternallyManaged { get; set; }
}
