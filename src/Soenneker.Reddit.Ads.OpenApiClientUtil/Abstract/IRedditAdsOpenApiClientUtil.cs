using Soenneker.Reddit.Ads.OpenApiClient;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Reddit.Ads.OpenApiClientUtil.Abstract;

/// <summary>
/// Exposes a cached OpenAPI client instance.
/// </summary>
public interface IRedditAdsOpenApiClientUtil: IDisposable, IAsyncDisposable
{
    /// <summary>Gets the shared Reddit Ads client. Authentication is initialized from Reddit:Ads:AccessToken.</summary>
    ValueTask<RedditAdsOpenApiClient> Get(CancellationToken cancellationToken = default);
}
