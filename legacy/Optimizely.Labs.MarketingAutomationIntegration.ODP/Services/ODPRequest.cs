using Newtonsoft.Json;
using System.Collections.Generic;

namespace Optimizely.Labs.MarketingAutomationIntegration.ODP.Services
{
    public class ODPRequest
    {
        [JsonProperty("attributes")]
        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
    }
}