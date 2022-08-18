using System.Configuration;

namespace Optimizely.Labs.MarketingAutomationIntegration.ODP
{
    public class SettingsOptions
    {
        static SettingsOptions()
        {
            OdpBaseEndPoint = ConfigurationManager.AppSettings["ma-odp-odpbaseendpoint"];
            CustomerObjectName = ConfigurationManager.AppSettings["ma-odp-customerobjectname"];
            APIKey = ConfigurationManager.AppSettings["ma-odp-apikey"];
        }

        public static string OdpBaseEndPoint { get; }

        public static string CustomerObjectName { get; }

        public static string APIKey { get; }
    }
}