using EPiServer.Forms.Core.Internal.ExternalSystem;
using EPiServer.ServiceLocation;
using Optimizely.Labs.MarketingAutomationIntegration.ODP.Services;
using System.Collections.Generic;
using System.Linq;

namespace Optimizely.Labs.MarketingAutomationIntegration.ODP
{
    public class ODPExternalSystem : IExternalSystem
    {
        public const string DataSourceKey = "ODPListing";

        private readonly Injected<ODPService> _odpService;

        public ODPExternalSystem()
        {
        }

        public virtual string Id => DataSourceKey;

        public virtual IEnumerable<IDatasource> Datasources
        {
            get
            {
                var customerDataSource = new Datasource()
                {
                    Name = SettingsOptions.CustomerObjectName,
                    Id = SettingsOptions.CustomerObjectName,
                    OwnerSystem = this
                };
                var fields = this._odpService.Service.GetFields(SettingsOptions.CustomerObjectName);
                if (fields.Any())
                {
                    customerDataSource.Columns = fields
                        .OrderBy(x => x.DisplayName)
                        .ToDictionary(x => x.Name, x => x.DisplayName);
                }
                return new List<IDatasource> { customerDataSource };
            }
        }
    }
}