using EPiServer.Forms.Core.Internal.ExternalSystem;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using Episerver.Marketing.Connector.Framework;
using EPiServer.ServiceLocation;
using Microsoft.Extensions.DependencyInjection;
using Optimizely.Labs.MarketingAutomationIntegration.ODP.Services;

namespace Optimizely.Labs.MarketingAutomationIntegration.ODP
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule),
        typeof(MarketingConnectorInitialization))]
    public class ODPInitilization : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            //context.Services.AddSingleton<IODPService,ODPService>();
        }

        public void Initialize(InitializationEngine context)
        {
            ExternalSystemService externalSystemService = context.Locate.Advanced.GetInstance<ExternalSystemService>();
            ODPExternalSystem odpExternalSystem = new ODPExternalSystem();
            externalSystemService.RegisterExternalSystem(odpExternalSystem);
        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}