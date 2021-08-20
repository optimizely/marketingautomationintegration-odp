using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Optimizely.Labs.MarketingAutomationIntegration.ODP.Services
{
    public class ListingRoot
    {
        [JsonProperty("lists")]
        public List<ListObject> Lists { get; set; } = new List<ListObject>();
    }

    public class ListObject
    {
        [JsonProperty("list_id")]
        public string ListId { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("data_source_type")]
        public string DataSourceType { get; set; }

        [JsonProperty("data_source_version")]
        public string DataSourceVersion { get; set; }

        [JsonProperty("data_source")]
        public string DataSource { get; set; }

        [JsonProperty("last_modified_at")]
        public int LastModifiedAt { get; set; }

        [JsonProperty("data_source_details")]
        public string DataSourceDetails { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("data_source_instance")]
        public string DataSourceInstance { get; set; }
    }
}