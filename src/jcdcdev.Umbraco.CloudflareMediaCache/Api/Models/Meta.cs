using System.Text.Json.Serialization;

namespace jcdcdev.Umbraco.CloudflareMediaCache.Api.Models;

public class Meta
{
    [JsonPropertyName("step")] public int Step { get; set; }

    [JsonPropertyName("custom_certificate_quota")]
    public int CustomCertificateQuota { get; set; }

    [JsonPropertyName("page_rule_quota")] public int PageRuleQuota { get; set; }

    [JsonPropertyName("phishing_detected")]
    public bool PhishingDetected { get; set; }

    [JsonPropertyName("multiple_railguns_allowed")]
    public bool MultipleRailgunsAllowed { get; set; }
}
