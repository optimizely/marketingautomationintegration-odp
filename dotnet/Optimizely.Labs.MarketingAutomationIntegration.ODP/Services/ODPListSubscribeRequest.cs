using Newtonsoft.Json;

namespace Optimizely.Labs.MarketingAutomationIntegration.ODP.Services
{
    public class ODPListSubscribeRequest
    {
        [JsonProperty("list_id")]
        public string ListId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("subscribed")]
        public bool Subscribed { get; set; }
    }
}