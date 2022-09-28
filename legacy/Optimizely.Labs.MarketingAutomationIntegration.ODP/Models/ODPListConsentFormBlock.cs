using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Forms.Implementation.Elements;
using System.ComponentModel.DataAnnotations;

namespace Optimizely.Labs.MarketingAutomationIntegration.ODP.Models
{
    [ContentType(DisplayName = "ODP List Consent Form Block", GroupName = "ODP", GUID = "A4661D51-4160-4EB3-845A-B287B5CFBB26", Description = "")]
    public class ODPListConsentFormBlock : TextboxElementBlock
    {
        [ScaffoldColumn(false)]
        public override string Validators { get; set; }

        [ScaffoldColumn(false)]
        public override string Description { get; set; }

        [ScaffoldColumn(false)]
        public override string PlaceHolder { get; set; }

        [ScaffoldColumn(false)]
        public override string PredefinedValue { get; set; }
    }
}