using Newtonsoft.Json;

namespace Optimizely.Labs.MarketingAutomationIntegration.ODP.Services
{
    public class ODPObject
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
    }
}