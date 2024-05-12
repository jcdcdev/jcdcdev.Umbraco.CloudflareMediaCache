using System.Text.Json.Serialization;

namespace jcdcdev.Umbraco.CloudflareMediaCache.Api.Models;

public class Plan
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("price")] public int Price { get; set; }

    [JsonPropertyName("currency")] public string Currency { get; set; }

    [JsonPropertyName("frequency")] public string Frequency { get; set; }

    [JsonPropertyName("is_subscribed")] public bool IsSubscribed { get; set; }

    [JsonPropertyName("can_subscribe")] public bool CanSubscribe { get; set; }

    [JsonPropertyName("legacy_id")] public string LegacyId { get; set; }

    [JsonPropertyName("legacy_discount")] public bool LegacyDiscount { get; set; }

    [JsonPropertyName("externally_managed")]
    public bool ExternallyManaged { get; set; }
}
