using Newtonsoft.Json;
using System.Collections.Generic;

namespace Optimizely.Labs.MarketingAutomationIntegration.ODP.Services
{
    public class ODPCustomerIdentifierResponse
    {
        [JsonProperty("events")]
        public List<ODPResponseItem> Events { get; set; } = new List<ODPResponseItem>();

        [JsonProperty("customers")]
        public List<ODPResponseItem> Customers { get; set; } = new List<ODPResponseItem>();
    }

    public class ODPResponseItem
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}