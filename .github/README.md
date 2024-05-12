# jcdcdev.Umbraco.CloudflareMediaCache

[![Umbraco Version](https://img.shields.io/badge/Umbraco-10.4+-%233544B1?style=flat&logo=umbraco)](https://umbraco.com/products/umbraco-cms/)
[![NuGet](https://img.shields.io/nuget/vpre/jcdcdev.Umbraco.CloudflareMediaCache?color=0273B3)](https://www.nuget.org/packages/jcdcdev.Umbraco.CloudflareMediaCache)
[![GitHub license](https://img.shields.io/github/license/jcdcdev/jcdcdev.Umbraco.CloudflareMediaCache?color=8AB803)](../LICENSE)
[![Downloads](https://img.shields.io/nuget/dt/jcdcdev.Umbraco.CloudflareMediaCache?color=cc9900)](https://www.nuget.org/packages/jcdcdev.Umbraco.CloudflareMediaCache/)

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
        "ApiToken": "API_TOKEN",,
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
|-----------|------------------------------------------|
| `ZoneId`  | The Cloudflare Zone ID                   |
| `Key`     | The Cloudflare API Key                   |
| `Enabled` | Whether to enable functionality          |
| `Mode`    | The cache mode (All, Prefix)             |
| `MaxAge`  | The max-age for cache headers  (seconds) |

## Contributing

Contributions to this package are most welcome! Please read the [Contributing Guidelines](CONTRIBUTING.md).

## Acknowledgments (thanks!)

- LottePitcher - [opinionated-package-starter](https://github.com/LottePitcher/opinionated-package-starter)
- jcdcdev - [jcdcdev.Umbraco.PackageTemplate](https://github.com/jcdcdev/jcdcdev.Umbraco.PackageTemplate)