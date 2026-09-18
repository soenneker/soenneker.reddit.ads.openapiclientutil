using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Kiota.Serialization.Json;
using Soenneker.Reddit.Ads.HttpClients.Abstract;
using Soenneker.Reddit.Ads.OpenApiClient.Models;

namespace Soenneker.Reddit.Ads.OpenApiClientUtil.Tests;

public sealed class RedditAdsRequestTests
{
    [Test]
    [Arguments(null, "https://ads-api.reddit.com/api/v3/me")]
    [Arguments("https://example.test/custom/v3/", "https://example.test/custom/v3/me")]
    public async Task Requests_use_bearer_token_and_configured_base_url(string? baseUrl, string expectedUrl)
    {
        IConfiguration config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Reddit:Ads:AccessToken"] = "test-token",
            ["Reddit:Ads:ClientBaseUrl"] = baseUrl
        }).Build();
        var handler = new RecordingHandler();
        using var httpClient = new HttpClient(handler);
        await using var util = new RedditAdsOpenApiClientUtil(new HttpClientStub(httpClient), config);
        var client = await util.Get();
        if (!ReferenceEquals(client, await util.Get()))
            throw new Exception("Expected a shared client instance.");
        await client.Me.GetAsync();
        if (handler.Url != expectedUrl || handler.Authorization != "Bearer test-token")
            throw new Exception($"Unexpected request: {handler.Url}, {handler.Authorization}");
    }

    [Test]
    public async Task Image_creative_asset_deserializes_its_properties()
    {
        var json = Encoding.UTF8.GetBytes("{\"name\":\"test image\",\"media\":{\"id\":\"asset-id\"}}");
        var node = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", new System.IO.MemoryStream(json));
        var asset = node.GetObjectValue(ComponentsSchemaPostCreativeAssetsImageCreativeAsset.CreateFromDiscriminatorValue);
        if (asset?.Name != "test image" || asset.Media is null)
            throw new Exception("Image creative assets must deserialize through the generated factory.");
    }

    private sealed class RecordingHandler : HttpMessageHandler
    {
        public string? Url { get; private set; }
        public string? Authorization { get; private set; }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Url = request.RequestUri?.AbsoluteUri;
            Authorization = request.Headers.Authorization?.ToString();
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{}", Encoding.UTF8, "application/json")
            });
        }
    }

    private sealed class HttpClientStub(HttpClient client) : IRedditAdsOpenApiHttpClient
    {
        public ValueTask<HttpClient> Get(CancellationToken cancellationToken = default) => ValueTask.FromResult(client);
        public void Dispose() { }
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}
