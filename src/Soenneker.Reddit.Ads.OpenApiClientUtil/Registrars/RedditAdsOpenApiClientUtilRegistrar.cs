using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Reddit.Ads.HttpClients.Registrars;
using Soenneker.Reddit.Ads.OpenApiClientUtil.Abstract;

namespace Soenneker.Reddit.Ads.OpenApiClientUtil.Registrars;

/// <summary>
/// Registers the OpenAPI client utility for dependency injection.
/// </summary>
public static class RedditAdsOpenApiClientUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="RedditAdsOpenApiClientUtil"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddRedditAdsOpenApiClientUtilAsSingleton(this IServiceCollection services)
    {
        services.AddRedditAdsOpenApiHttpClientAsSingleton()
                .TryAddSingleton<IRedditAdsOpenApiClientUtil, RedditAdsOpenApiClientUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="RedditAdsOpenApiClientUtil"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddRedditAdsOpenApiClientUtilAsScoped(this IServiceCollection services)
    {
        services.AddRedditAdsOpenApiHttpClientAsSingleton()
                .TryAddScoped<IRedditAdsOpenApiClientUtil, RedditAdsOpenApiClientUtil>();

        return services;
    }
}
