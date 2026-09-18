using Soenneker.Reddit.Ads.OpenApiClientUtil.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Reddit.Ads.OpenApiClientUtil.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class RedditAdsOpenApiClientUtilTests : HostedUnitTest
{
    private readonly IRedditAdsOpenApiClientUtil _openapiclientutil;

    public RedditAdsOpenApiClientUtilTests(Host host) : base(host)
    {
        _openapiclientutil = Resolve<IRedditAdsOpenApiClientUtil>(true);
    }

    [Test]
    public void Default()
    {

    }
}
