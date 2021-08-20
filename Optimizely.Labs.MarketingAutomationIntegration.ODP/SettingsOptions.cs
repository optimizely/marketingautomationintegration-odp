using EPiServer.ServiceLocation;

namespace Optimizely.Labs.MarketingAutomationIntegration.ODP
{
    [Options]
    public class SettingsOptions
    {
        public string CustomerObjectName { get; set; } = "customers";

        public string APIKey { get; set; }
    }
}