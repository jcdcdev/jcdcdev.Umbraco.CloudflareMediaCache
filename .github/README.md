# Cloudflare Media Cache

[![Documentation](https://jcdc.dev/badge/Documentation/primary/book)](https://docs.jcdc.dev/jcdcdev-umbraco-cloudflaremediacache/latest)
[![Umbraco Marketplace](https://jcdc.dev/badge/Umbraco%20Marketplace/umbraco/umbraco)](https://marketplace.umbraco.com/package/jcdcdev.Umbraco.CloudflareMediaCache)
[![GitHub](https://jcdc.dev/badge/GitHub/github/github)](https://github.com/jcdcdev/jcdcdev.Umbraco.CloudflareMediaCache)
[![NuGet package downloads](https://jcdc.dev/badge/nuget/jcdcdev.Umbraco.CloudflareMediaCache)](https://www.nuget.org/packages/jcdcdev.Umbraco.CloudflareMediaCache)
[![Project Website](https://jcdc.dev/badge/Project%20Website/primary/laptop)](https://jcdc.dev/umbraco-packages/cloudflare-media-cache)


- Automatically purge Cloudflare cache when media saved
    - Purge by prefix (Cloudflare Pro & Enterprise)
    - Purge all (All Cloudflare plans)
- Adds cache headers when serving media
    - Respects image cropper

> [!IMPORTANT]
> Version 16 will only receive security updates and no new features.

> Please review the [security policy](https://github.com/jcdcdev/jcdcdev.Umbraco.CloudflareMediaCache?tab=security-ov-file#supported-versions) for more information.

## Installation

### Install Package

```powershell
dotnet add package jcdcdev.Umbraco.CloudflareMediaCache
```

## Configuration

Add the following section to your `appsettings.json`:

```json
{
  "Cloudflare": {
    "Media": {
      "Cache": {
        "ZoneId": "ZONE_ID",
        "ApiToken": "API_TOKEN",
        "Enabled": true,
        "Mode": "All",
        "MaxAge": 2592000
      }
    }
  }
}
```
### Options

| Option    | Description                              |
| --------- | ---------------------------------------- |
| `ZoneId`  | The Cloudflare Zone ID                   |
| `Key`     | The Cloudflare API Key                   |
| `Enabled` | Whether to enable functionality          |
| `Mode`    | The cache mode (All, Prefix)             |
| `MaxAge`  | The max-age for cache headers  (seconds) |

## Security

> [!NOTE]
> This project takes security and support seriously.
> Please visit the [Security](https://github.com/jcdcdev/jcdcdev.Umbraco.CloudflareMediaCache?tab=security-ov-file) page for more information.



## Contributing

Contributions to this package are most welcome! Please visit the [Contributing](https://github.com/jcdcdev/jcdcdev.Umbraco.CloudflareMediaCache/contribute) page.

## Acknowledgements

Thank you to the following projects and individuals for their contributions. High five, you rock! 🤘🦄

- LottePitcher - [opinionated-package-starter](https://github.com/LottePitcher/opinionated-package-starter)



