using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optimizely.Labs.MarketingAutomationIntegration.ODP.Services;

namespace Optimizely.Labs.MarketingAutomationIntegration.ODP;

public static class ServiceCollectionHelpers
{
    public static IServiceCollection AddMarketingAutomationIntegrationODP(
        this IServiceCollection services,
        IConfiguration configurationRoot)
    {
        services.Configure<MAIOdpSettings>(
            configurationRoot.GetSection(
                key: nameof(MAIOdpSettings)));
        services.AddSingleton<IODPService, ODPService>();
        return services;
    }
}