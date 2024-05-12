using jcdcdev.Umbraco.Core.Extensions;
using Umbraco.Cms.Core.Manifest;

namespace jcdcdev.Umbraco.CloudflareMediaCache.Composing;

internal class ManifestFilter : IManifestFilter
{
    public void Filter(List<PackageManifest> manifests)
    {
        manifests.Add(new PackageManifest
        {
            PackageName = "jcdcdev.Umbraco.CloudflareMediaCache",
            Version = EnvironmentExtensions.CurrentAssemblyVersion()?.ToSemVer()?.ToString() ?? "0.1.0",
            AllowPackageTelemetry = true
        });
    }
}