using System;
using EPiServer.Forms.Core.Internal.ExternalSystem;
using EPiServer.ServiceLocation;
using Optimizely.Labs.MarketingAutomationIntegration.ODP.Services;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;

namespace Optimizely.Labs.MarketingAutomationIntegration.ODP
{
    public class ODPExternalSystem : IExternalSystem
    {
        public const string DataSourceKey = "ODPListing";

        private readonly Injected<IODPService> _odpService;
        private Injected<IOptions<MAIOdpSettings>> _config;

        public virtual string Id => DataSourceKey;

        public virtual IEnumerable<IDatasource> Datasources
        {
            get
            {
                var customerDataSource = new Datasource()
                {
                    Name = _config.Service.Value.CustomerObjectName,
                    Id = _config.Service.Value.CustomerObjectName,
                    OwnerSystem = this
                };
                var fields = this._odpService.Service.GetFields(_config.Service.Value.CustomerObjectName);
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