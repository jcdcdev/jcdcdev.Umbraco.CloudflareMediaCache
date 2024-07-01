using jcdcdev.Umbraco.CloudflareMediaCache.Api;
using jcdcdev.Umbraco.CloudflareMediaCache.Api.Models;
using jcdcdev.Umbraco.CloudflareMediaCache.Models;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.HealthChecks;

namespace jcdcdev.Umbraco.CloudflareMediaCache;

[HealthCheck(HealthCheckId, HealthCheckName, Description = "Checks the Cloudflare API to ensure it is available.", Group = "Cloudflare")]
public class CloudflareCacheHealthCheck(IOptions<CloudflareCacheOptions> options, ICloudflareCacheApiClient client) : HealthCheck
{
    private const string HealthCheckId = "3f0bbea9-d82e-4805-b9c0-aae4c8bf96eb";
    private const string HealthCheckName = "Cloudflare Cache Health Check";
    private readonly CloudflareCacheOptions _options = options.Value;

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

        var response = await client.GetZoneDetails();
        var zoneResponse = response.Result;
        if (!response.Success)
        {
            var description = "Check your ZoneId and Key are correct.";
            if (zoneResponse?.Errors.Any() ?? false)
            {
                description += $"\n\n{string.Join("\n", zoneResponse?.Errors!)}";
            }

            statuses.Add(new HealthCheckStatus("Cloudflare API is not available.")
            {
                Description = description,
                ResultType = StatusResultType.Error
            });
            return statuses;
        }

        statuses.Add(GetPermissionStatus(zoneResponse));
        var planStatus = GetPlanStatus(zoneResponse, _options.Mode);
        if (planStatus != null)
        {
            statuses.Add(planStatus);
        }

        return statuses;
    }

    private static HealthCheckStatus GetPermissionStatus(ZoneResponse? zoneResponse)
    {
        var name = zoneResponse?.Result.Name ?? "Unknown";
        var permissions = string.Join("\n", zoneResponse?.Result.Permissions ?? []);
        return new HealthCheckStatus("Cloudflare API is available.")
        {
            Description = $"Zone: {name}\n{permissions}",
            ResultType = StatusResultType.Success
        };
    }

    private static HealthCheckStatus? GetPlanStatus(ZoneResponse? zoneResponse, PurgeCacheMode mode)
    {
        if (zoneResponse.IsFreePlan())
        {
            return GetFreePlanStatus(mode);
        }

        switch (mode)
        {
            case PurgeCacheMode.All:
                return
                    new HealthCheckStatus($"Cache Mode {mode} - Pro or Enterprise Cloudflare Plan")
                    {
                        Description =
                            "You are using the Pro or Enterprise Cloudflare plan. This plan supports purging by Prefix (Per media URLs). Enable it in settings",
                        ResultType = StatusResultType.Warning
                    };
            case PurgeCacheMode.Prefix:
                return
                    new HealthCheckStatus($"Cache Mode {mode} - Pro or Enterprise Cloudflare Plan")
                    {
                        Description =
                            "You are using the Pro or Enterprise Cloudflare plan. This plan supports purging by Prefix (Per media URLs).",
                        ResultType = StatusResultType.Success
                    };
            case PurgeCacheMode.Unknown:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return null;
    }

    private static HealthCheckStatus? GetFreePlanStatus(PurgeCacheMode mode)
    {
        switch (mode)
        {
            case PurgeCacheMode.All:
                return new HealthCheckStatus($"Cache Mode {mode} - Free Cloudflare Plan")
                {
                    Description =
                        "You are using the free Cloudflare plan. This plan does not support purging by URL. You can still use the Media Cache, but you will need to purge the entire cache when you update media.",
                    ResultType = StatusResultType.Info
                };
            case PurgeCacheMode.Prefix:
                return new HealthCheckStatus($"Cache Mode {mode} - Free Cloudflare Plan")
                {
                    Description =
                        "You are using the free Cloudflare plan. This plan does not support purging by Prefix (Per media URLs). You can still use the Media Cache, but you will need to purge the entire cache when you update media.",
                    ResultType = StatusResultType.Error
                };
            case PurgeCacheMode.Unknown:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return null;
    }


    public override HealthCheckStatus ExecuteAction(HealthCheckAction action) => new("How did you get here?")
    {
        ResultType = StatusResultType.Info
    };
}

public static class A
{
    public static bool IsFreePlan(this ZoneResponse? response) => response?.Result.Plan.LegacyId == "free";
}
