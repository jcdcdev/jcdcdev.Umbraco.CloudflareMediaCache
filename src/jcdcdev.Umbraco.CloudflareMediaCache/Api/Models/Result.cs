using System.Text.Json.Serialization;

namespace jcdcdev.Umbraco.CloudflareMediaCache.Api.Models;

public class Result
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("status")] public string Status { get; set; }

    [JsonPropertyName("paused")] public bool Paused { get; set; }

    [JsonPropertyName("type")] public string Type { get; set; }

    [JsonPropertyName("development_mode")] public int DevelopmentMode { get; set; }

    [JsonPropertyName("name_servers")] public List<string> NameServers { get; set; }

    [JsonPropertyName("original_name_servers")]
    public List<string> OriginalNameServers { get; set; }

    [JsonPropertyName("original_registrar")]
    public string OriginalRegistrar { get; set; }

    [JsonPropertyName("original_dnshost")] public string OriginalDnsHost { get; set; }

    [JsonPropertyName("modified_on")] public string ModifiedOn { get; set; }

    [JsonPropertyName("created_on")] public string CreatedOn { get; set; }

    [JsonPropertyName("activated_on")] public string ActivatedOn { get; set; }

    [JsonPropertyName("meta")] public Meta Meta { get; set; }

    [JsonPropertyName("owner")] public Owner Owner { get; set; }

    [JsonPropertyName("account")] public Account Account { get; set; }

    [JsonPropertyName("tenant")] public Tenant Tenant { get; set; }

    [JsonPropertyName("tenant_unit")] public TenantUnit TenantUnit { get; set; }

    [JsonPropertyName("permissions")] public List<string> Permissions { get; set; }

    [JsonPropertyName("plan")] public Plan Plan { get; set; }
}
