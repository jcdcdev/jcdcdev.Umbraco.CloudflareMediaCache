using System.Text.Json.Serialization;

namespace jcdcdev.Umbraco.CloudflareMediaCache.Api.Models;

public class Result
{
    [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;

    [JsonPropertyName("status")] public string Status { get; set; } = string.Empty;

    [JsonPropertyName("paused")] public bool Paused { get; set; }

    [JsonPropertyName("type")] public string Type { get; set; } = string.Empty;

    [JsonPropertyName("development_mode")] public int DevelopmentMode { get; set; }

    [JsonPropertyName("name_servers")] public List<string> NameServers { get; set; } = [];

    [JsonPropertyName("original_name_servers")]
    public List<string> OriginalNameServers { get; set; } = [];

    [JsonPropertyName("original_registrar")]
    public string OriginalRegistrar { get; set; } = string.Empty;

    [JsonPropertyName("original_dnshost")] public string OriginalDnsHost { get; set; } = string.Empty;

    [JsonPropertyName("modified_on")] public string ModifiedOn { get; set; } = string.Empty;

    [JsonPropertyName("created_on")] public string CreatedOn { get; set; } = string.Empty;

    [JsonPropertyName("activated_on")] public string ActivatedOn { get; set; } = string.Empty;

    [JsonPropertyName("meta")] public Meta Meta { get; set; } = new();

    [JsonPropertyName("owner")] public Owner Owner { get; set; } = new();

    [JsonPropertyName("account")] public Account Account { get; set; } = new();

    [JsonPropertyName("tenant")] public Tenant Tenant { get; set; } = new();

    [JsonPropertyName("tenant_unit")] public TenantUnit TenantUnit { get; set; } = new();

    [JsonPropertyName("permissions")] public List<string> Permissions { get; set; } = [];

    [JsonPropertyName("plan")] public Plan Plan { get; set; } = new();
}
