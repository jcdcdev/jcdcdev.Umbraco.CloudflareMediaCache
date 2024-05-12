using jcdcdev.Umbraco.CloudflareMediaCache.Api;
using jcdcdev.Umbraco.CloudflareMediaCache.Api.Models;
using jcdcdev.Umbraco.CloudflareMediaCache.Models;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.HealthChecks;

namespace jcdcdev.Umbraco.CloudflareMediaCache;

[HealthCheck(HealthCheckId, HealthCheckName, Description = "Checks the Cloudflare API to ensure it is available.",
    Group = "Cloudflare")]
public class CloudflareCacheHealthCheck : HealthCheck
{
    private const string HealthCheckId = "3f0bbea9-d82e-4805-b9c0-aae4c8bf96eb";
    private const string HealthCheckName = "Cloudflare Cache Health Check";
    private readonly CloudflareCacheOptions _options;
    private readonly ICloudflareCacheApiClient _client;

    public CloudflareCacheHealthCheck(IOptions<CloudflareCacheOptions> options, ICloudflareCacheApiClient client)
    {
        _client = client;
        _options = options.Value;
    }

    public override async Task<IEnumerable<HealthCheckStatus>> GetStatus()
    {
        if (!_options.Enabled)
        {
            return new[]
            {
                new HealthCheckStatus("Cloudflare Cache is not enabled.")
                {
                    Description = "Enable Cloudflare Cache in settings.",
                    ResultType = StatusResultType.Info
                }
            };
        }

        var statuses = new List<HealthCheckStatus>();

        if (_options.Mode == PurgeCacheMode.Unknown)
        {
            statuses.Add(new HealthCheckStatus("Cloudflare Cache Mode is not set.")
            {
                Description = "Set Cloudflare Cache Mode in settings.",
                ResultType = StatusResultType.Error
            });
        }

        var response = await _client.GetZoneDetails();
        if (response.Success)
        {
            statuses.Add(new HealthCheckStatus("Cloudflare API is available.")
            {
                Description = "Zone: " + response.Result?.Result?.Name + "\n" +
                              string.Join("\n", response.Result?.Result?.Permissions!),
                ResultType = StatusResultType.Success
            });

            if (response.Result?.Result?.Plan.LegacyId == "free")
            {
                switch (_options.Mode)
                {
                    case PurgeCacheMode.All:
                        statuses.Add(new HealthCheckStatus($"Cache Mode {_options.Mode} - Free Cloudflare Plan")
                        {
                            Description =
                                "You are using the free Cloudflare plan. This plan does not support purging by URL. You can still use the Media Cache, but you will need to purge the entire cache when you update media.",
                            ResultType = StatusResultType.Info
                        });
                        break;
                    case PurgeCacheMode.Prefix:
                        statuses.Add(new HealthCheckStatus($"Cache Mode {_options.Mode} - Free Cloudflare Plan")
                        {
                            Description =
                                "You are using the free Cloudflare plan. This plan does not support purging by Prefix (Per media URLs). You can still use the Media Cache, but you will need to purge the entire cache when you update media.",
                            ResultType = StatusResultType.Error
                        });
                        break;
                }
            }
            else
            {
                switch (_options.Mode)
                {
                    case PurgeCacheMode.All:
                        statuses.Add(
                            new HealthCheckStatus($"Cache Mode {_options.Mode} - Pro or Enterprise Cloudflare Plan")
                            {
                                Description =
                                    "You are using the Pro or Enterprise Cloudflare plan. This plan supports purging by Prefix (Per media URLs). Enable it in settings",
                                ResultType = StatusResultType.Warning
                            });
                        break;
                    case PurgeCacheMode.Prefix:
                        statuses.Add(
                            new HealthCheckStatus($"Cache Mode {_options.Mode} - Pro or Enterprise Cloudflare Plan")
                            {
                                Description =
                                    "You are using the Pro or Enterprise Cloudflare plan. This plan supports purging by Prefix (Per media URLs).",
                                ResultType = StatusResultType.Success
                            });
                        break;
                }
            }
        }
        else
        {
            var description = "Check your ZoneId and Key are correct.";
            if (response.Result?.Errors.Any() ?? false)
            {
                description += $"\n\n{string.Join("\n", response.Result?.Errors!)}";
            }

            statuses.Add(new HealthCheckStatus("Cloudflare API is not available.")
            {
                Description = description,
                ResultType = StatusResultType.Error
            });
        }

        return statuses;
    }

    public override HealthCheckStatus ExecuteAction(HealthCheckAction action) => new("How did you get here?")
    {
        ResultType = StatusResultType.Info
    };
}