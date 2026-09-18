using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Soenneker.Extensions.Configuration;
using Soenneker.Extensions.ValueTask;
using Soenneker.Reddit.Ads.HttpClients.Abstract;
using Soenneker.Reddit.Ads.OpenApiClientUtil.Abstract;
using Soenneker.Reddit.Ads.OpenApiClient;
using Soenneker.Kiota.GenericAuthenticationProvider;
using Soenneker.Utils.AsyncSingleton;

namespace Soenneker.Reddit.Ads.OpenApiClientUtil;

public sealed class RedditAdsOpenApiClientUtil : IRedditAdsOpenApiClientUtil
{
    private readonly AsyncSingleton<RedditAdsOpenApiClient> _client;

    public RedditAdsOpenApiClientUtil(IRedditAdsOpenApiHttpClient httpClientUtil, IConfiguration configuration)
    {
        _client = new AsyncSingleton<RedditAdsOpenApiClient>(async token =>
        {
            HttpClient httpClient = await httpClientUtil.Get(token).NoSync();

            var apiKey = configuration.GetValueStrict<string>("Reddit:Ads:AccessToken");
            string authHeaderName = configuration["Reddit:Ads:AuthHeaderName"] ?? "Authorization";
            string authHeaderValueTemplate = configuration["Reddit:Ads:AuthHeaderValueTemplate"] ?? "Bearer {token}";
            string authHeaderValue = authHeaderValueTemplate.Replace("{token}", apiKey, StringComparison.Ordinal);

            var requestAdapter = new HttpClientRequestAdapter(new GenericAuthenticationProvider(headerName: authHeaderName, headerValue: authHeaderValue),
                httpClient: httpClient)
            {
                BaseUrl = (configuration["Reddit:Ads:ClientBaseUrl"] ?? httpClient.BaseAddress?.AbsoluteUri ?? "https://ads-api.reddit.com/api/v3").TrimEnd('/')
            };

            return new RedditAdsOpenApiClient(requestAdapter);
        });
    }

    public ValueTask<RedditAdsOpenApiClient> Get(CancellationToken cancellationToken = default)
    {
        return _client.Get(cancellationToken);
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return _client.DisposeAsync();
    }
}
