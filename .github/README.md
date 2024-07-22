# jcdcdev.Umbraco.CloudflareMediaCache

[![Umbraco Marketplace](https://img.shields.io/badge/Umbraco-Marketplace-%233544B1?style=flat&logo=umbraco)](https://marketplace.umbraco.com/package/jcdcdev.umbraco.cloudflaremediacache)
[![GitHub License](https://img.shields.io/github/license/jcdcdev/jcdcdev.Umbraco.CloudflareMediaCache?color=8AB803&label=License&logo=github)](https://github.com/jcdcdev/jcdcdev.Umbraco.CloudflareMediaCache/blob/main/LICENSE)
[![NuGet Downloads](https://img.shields.io/nuget/dt/jcdcdev.Umbraco.CloudflareMediaCache?color=cc9900&label=Downloads&logo=nuget)](https://www.nuget.org/packages/jcdcdev.Umbraco.CloudflareMediaCache/)

## Features

- Automatically purge Cloudflare cache when media saved
    - Purge by prefix (Cloudflare Pro & Enterprise)
    - Purge all (All Cloudflare plans)
- Adds cache headers when serving media
    - Respects image cropper

## Quick Start

### Install Package

```csharp
dotnet add package jcdcdev.Umbraco.CloudflareMediaCache
```

### Get Cloudflare Zone ID & API Key

You will need a Cloudflare Zone ID and API Key to use this package.

- Zone ID: [Find your Zone ID](https://developers.cloudflare.com/fundamentals/setup/find-account-and-zone-ids/)
- API Key: [Create an API Key](https://developers.cloudflare.com/api/tokens/create)

Note: The API Key should have the following permission:
- Zone.Cache Purge

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

## Contributing

Contributions to this package are most welcome! Please read the [Contributing Guidelines](https://github.com/jcdcdev/jcdcdev.Umbraco.CloudflareMediaCache/blob/main/.github/CONTRIBUTING.md).

## Acknowledgments (thanks!)

- LottePitcher - [opinionated-package-starter](https://github.com/LottePitcher/opinionated-package-starter)
- jcdcdev - [jcdcdev.Umbraco.PackageTemplate](https://github.com/jcdcdev/jcdcdev.Umbraco.PackageTemplate)
