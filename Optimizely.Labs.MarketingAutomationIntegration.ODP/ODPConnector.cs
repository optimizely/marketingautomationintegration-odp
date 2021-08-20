using System;
using System.Collections.Generic;
using System.Linq;
using Episerver.Marketing.Common.Exceptions;
using Episerver.Marketing.Connector.Framework;
using Episerver.Marketing.Connector.Framework.Data;
using EPiServer.ServiceLocation;
using MarketingAutomation.ODP.Services;

namespace MarketingAutomation.ODP
{
    public class ODPConnector : IMarketingConnector
    {
        internal static Guid ConnectorGuid = new Guid("3DC5D92C-436A-4D82-B913-A49A1D9209E3");

        internal static Dictionary<string, EntityProfile> EntityProfileStorage = new Dictionary<string, EntityProfile>();

        private IServiceLocator _locator;

        private readonly IODPDataMappingService _odpService;

        private readonly IMarketingConnectorManager _marketingConnectorManager;

        private ConnectorCredentials _connectorCredentials = null;

        private Guid _instanceId = ConnectorGuid;

        public ODPConnector()
        {
            _locator = ServiceLocator.Current;
            _odpService = _locator.GetInstance<IODPDataMappingService>();
            _marketingConnectorManager = _locator.GetInstance<IMarketingConnectorManager>();
        }

        public Guid Id { get { return ConnectorGuid; } }

        public Guid InstanceId { get { return _instanceId; } set { _instanceId = value; } }

        public string _Name = "ODPConnector";

        public string Name { get => _Name; set => _Name = value; }

        public IEnumerable<Field> GetDataSourceFields(long id)
        {
            IsConfigured();
            var database = this._odpService.GetDatabaseMapping(id);
            return _odpService.GetFieldMappings(database)
                .Select(x => new Field()
                {
                    Name = x.DisplayName,
                    Id = x.FieldDataId
                });
        }

        public IEnumerable<ConnectorDataSource> GetDataSources()
        {
            IsConfigured();
            var dataSources = this._odpService.GetDataMappings()
                .Select(x => new ConnectorDataSource
                {
                    Name = x.DatabaseDisplayName,
                    Id = x.DatabaseId
                });
            return dataSources.Any() ? dataSources : new List<ConnectorDataSource>();
        }

        public EntityProfile GetEntity(ConnectorDataSource dataSource, string entityId)
        {
            IsConfigured();
            return EntityProfileStorage[entityId];
        }

        public string CreateEntity(ConnectorDataSource dataSource, Dictionary<string, string> entityFields)
        {
            IsConfigured();
            var id = EntityProfileStorage.Count.ToString();
            EntityProfileStorage.Add(id, new EntityProfile() { Id = id, Fields = entityFields });
            return id;
        }

        public string CreateEntity(SubmissionTarget submissionTarget, Dictionary<string, string> entityFields)
        {
            IsConfigured();
            var id = EntityProfileStorage.Count.ToString();
            EntityProfileStorage.Add(id, new EntityProfile() { Id = id, Fields = entityFields });
            return id;
        }

        public string UpdateEntity(string entityId, ConnectorDataSource dataSource, Dictionary<string, string> entityFields)
        {
            IsConfigured();
            if (EntityProfileStorage.TryGetValue(entityId, out EntityProfile entity))
            {
                EntityProfileStorage.Remove(entityId);
                EntityProfileStorage.Add(entityId, new EntityProfile() { Id = entityId, Fields = entityFields });
            }
            return entityId;
        }

        public string UpdateEntity(string entityId, SubmissionTarget submissionTarget, Dictionary<string, string> entityFields)
        {
            IsConfigured();
            if (EntityProfileStorage.TryGetValue(entityId, out EntityProfile entity))
            {
                EntityProfileStorage.Remove(entityId);
                EntityProfileStorage.Add(entityId, new EntityProfile() { Id = entityId, Fields = entityFields });
            }
            return entityId;
        }

        private bool IsConfigured()
        {
            if (_instanceId != Guid.Empty)
            {
                if (_marketingConnectorManager == null && _connectorCredentials == null)
                {
                    var _connectorCredentials = _marketingConnectorManager.GetConnectorCredentials(Id.ToString(), InstanceId.ToString());
                    if (_connectorCredentials == null)
                    {
                        throw new ConfigurationInvalidException($"credentials not found for {Name}");
                    }
                }
            }
            else
            {
                throw new ConfigurationInvalidException("InstanceId not set");
            }

            return true;
        }
    }
}