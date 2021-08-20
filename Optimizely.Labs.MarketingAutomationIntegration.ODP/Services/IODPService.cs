using EPiServer.Framework.Cache;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;

namespace Optimizely.Labs.MarketingAutomationIntegration.ODP.Services
{
    public interface IODPService
    {
        ODPCustomerIdentifierResponse SaveNewIdentifier(string name, string displayName, string mergeConfidence, bool messaging);

        Dictionary<string, string> GetProfileInformation(string email);

        bool SaveProfileInformation(string email, Dictionary<string, string> attribs);

        List<ODPField> GetFields(string objectName);

        List<ListObject> GetLists();

        bool AddToList(ODPListSubscribeRequest request);
    }

    public class ODPService : IODPService
    {
        private const string BaseUrl = "https://api.zaius.com/";

        private readonly HttpClient client = new HttpClient();

        private readonly ISynchronizedObjectInstanceCache _objectInstanceCache;

        private readonly SettingsOptions _settingOptions;

        public ODPService(ISynchronizedObjectInstanceCache objectInstanceCache, SettingsOptions settingOptions)
        {
            _objectInstanceCache = objectInstanceCache;
            _settingOptions = settingOptions;
            this.client = new HttpClient()
            {
                BaseAddress = new System.Uri(BaseUrl)
            };
            client.DefaultRequestHeaders.Add("x-api-key", _settingOptions.APIKey);
        }

        public Dictionary<string, string> GetProfileInformation(string email)
        {
            var dictionary = new Dictionary<string, string>();
            var response = client.GetAsync($"v3/profiles?email={email}").Result;
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                var parsed = (JObject)JsonConvert.DeserializeObject(content);
                dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(parsed["attributes"].ToString());
            }

            return dictionary;
        }

        public Dictionary<string, string> GetProfileInformationByUserId(string userId)
        {
            var dictionary = new Dictionary<string, string>();
            var response = client.GetAsync($"v3/profiles?opticontentcloud_id={userId}").Result;
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                var parsed = (JObject)JsonConvert.DeserializeObject(content);
                dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(parsed["attributes"].ToString());
            }

            return dictionary;
        }

        public bool SaveProfileInformation(string email, Dictionary<string, string> values)
        {
            var request = new ODPRequest()
            {
                Attributes = values
            };
            var json = JsonConvert.SerializeObject(request);
            using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                using (var httpResponseMessage = client.PostAsync("v3/profiles", content).Result)
                {
                    return httpResponseMessage.IsSuccessStatusCode;
                }
            }
        }

        public bool SaveProfileInformationByUserId(string userId, Dictionary<string, string> values)
        {
            var request = new ODPRequest()
            {
                Attributes = values
            };
            var json = JsonConvert.SerializeObject(request);
            using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                using (var httpResponseMessage = client.PostAsync("v3/profiles", content).Result)
                {
                    return httpResponseMessage.IsSuccessStatusCode;
                }
            }
        }

        public ODPCustomerIdentifierResponse SaveNewIdentifier(string name, string displayName, string mergeConfidence, bool messaging)
        {
            var request = new ODPCustomerIdentifierRequest()
            {
                DisplayName = displayName,
                MergeConfidence = mergeConfidence,
                Messaging = messaging,
                Name = name
            };
            var json = JsonConvert.SerializeObject(request);
            using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                using (var httpResponseMessage = client.PostAsync("v3/schema/identifiers", content).Result)
                {
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        var jsonResponse = httpResponseMessage.Content.ReadAsStringAsync().Result;
                        return JsonConvert.DeserializeObject<ODPCustomerIdentifierResponse>(jsonResponse);
                    }
                }
            }
            return null;
        }

        public List<ODPField> GetFields(string objectName)
        {
            var list = _objectInstanceCache.Get<List<ODPField>>($"odp-{objectName}-fields", ReadStrategy.Immediate);
            if (list == null)
            {
                list = new List<ODPField>();
                var response = client.GetAsync($"v3/schema/objects/{objectName}/fields").Result;
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    list = JsonConvert.DeserializeObject<List<ODPField>>(content);

                    _objectInstanceCache.Insert($"odp-{objectName}-fields", list, new CacheEvictionPolicy(TimeSpan.FromMinutes(60), CacheTimeoutType.Absolute));
                }
            }
            return list;
        }

        public List<ListObject> GetLists()
        {
            var list = _objectInstanceCache.Get<List<ListObject>>("odp-objects", ReadStrategy.Immediate);
            if (list == null)
            {
                list = new List<ListObject>();
                var response = client.GetAsync($"v3/lists").Result;
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    list = JsonConvert.DeserializeObject<ListingRoot>(content).Lists;
                    _objectInstanceCache.Insert("odp-listobjects", list, new CacheEvictionPolicy(TimeSpan.FromMinutes(60), CacheTimeoutType.Absolute));
                }
            }
            return list;
        }

        public bool AddToList(ODPListSubscribeRequest request)
        {
            var json = JsonConvert.SerializeObject(request);

            using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                using (var httpResponseMessage = client.PostAsync("v3/lists/subscriptions", content).Result)
                {
                    return httpResponseMessage.IsSuccessStatusCode;
                }
            }
        }
    }
}