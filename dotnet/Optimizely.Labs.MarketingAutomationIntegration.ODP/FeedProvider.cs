using EPiServer.Forms.Core.Feed.Internal;
using EPiServer.ServiceLocation;
using System.Collections.Generic;

namespace Optimizely.Labs.MarketingAutomationIntegration.ODP
{
    public class FeedProvider : IFeedProvider
    {
        public IEnumerable<IFeed> GetFeeds() =>
            ServiceLocator.Current.GetAllInstances<IFeed>();
    }
}