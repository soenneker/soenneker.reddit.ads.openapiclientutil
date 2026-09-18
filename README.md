[![](https://img.shields.io/nuget/v/soenneker.reddit.ads.openapiclientutil.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.reddit.ads.openapiclientutil/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.reddit.ads.openapiclientutil/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.reddit.ads.openapiclientutil/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.reddit.ads.openapiclientutil.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.reddit.ads.openapiclientutil/)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Reddit.Ads.OpenApiClientUtil
### A thread-safe utility for obtaining Reddit Ads OpenApiClient singleton.

## Installation

```
dotnet add package Soenneker.Reddit.Ads.OpenApiClientUtil
```

## Usage

Register `services.AddRedditAdsOpenApiClientUtilAsSingleton()` from the
`Soenneker.Reddit.Ads.OpenApiClientUtil.Registrars` namespace, then inject
`IRedditAdsOpenApiClientUtil` from `.Abstract`:

```csharp
var client = await clientUtil.Get(cancellationToken);
var actor = await client.Me.GetAsync(cancellationToken: cancellationToken);
var ads = await client.Ad_accounts[adAccountId].Ads.GetAsync(cancellationToken: cancellationToken);
```

Set `Reddit:Ads:AccessToken` through configuration or the environment variable
`Reddit__Ads__AccessToken`. Supply an OAuth access token with the permissions your
operations require. Token acquisition and refresh are the application's responsibility;
this utility captures the token when its cached client is first created. Recreate the
service scope or provider when replacing a token.

Optional configuration under `Reddit:Ads`:

- `ClientBaseUrl`: defaults to `https://ads-api.reddit.com/api/v3`.
- `AuthHeaderName`: defaults to `Authorization`.
- `AuthHeaderValueTemplate`: defaults to `Bearer {token}`.

The utility owns the cached generated client. Do not dispose the shared HTTP client
returned by the HTTP client wrapper yourself.

## Local development

Keep the OpenApiClient and HttpClients repositories alongside this repository and run:

```sh
dotnet build -p:UseLocalRedditAdsProjects=true
```

This uses sibling project references before the first packages are published. Normal
builds use NuGet dependencies; publish OpenApiClient and HttpClients before this utility.
