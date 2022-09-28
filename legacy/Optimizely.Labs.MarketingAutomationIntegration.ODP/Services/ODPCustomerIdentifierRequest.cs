using Newtonsoft.Json;

namespace Optimizely.Labs.MarketingAutomationIntegration.ODP.Services
{
    public class ODPCustomerIdentifierRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("merge_confidence")]
        public string MergeConfidence { get; set; }

        [JsonProperty("messaging")]
        public bool Messaging { get; set; }
    }
}