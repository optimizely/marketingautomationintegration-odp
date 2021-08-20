using EPiServer;
using EPiServer.Core;
using EPiServer.Forms.Core.PostSubmissionActor;
using EPiServer.Forms.Implementation.Elements;
using EPiServer.ServiceLocation;
using Optimizely.Labs.MarketingAutomationIntegration.ODP.Models;
using Optimizely.Labs.MarketingAutomationIntegration.ODP.Services;
using System.Collections.Generic;
using System.Linq;

namespace Optimizely.Labs.MarketingAutomationIntegration.ODP
{
    public class ODPPostActor : PostSubmissionActorBase
    {
        private string _objectType;

        private readonly Injected<IODPService> _odpService;

        private readonly Injected<IContentLoader> ContentLoader;

        private IEnumerable<IContent> _formContents = Enumerable.Empty<IContent>();

        public override object Run(object input)
        {
            string str = string.Empty;
            if (SubmissionData == null)
            {
                return str;
            }

            bool validProfileSave = false;
            string email = string.Empty;
            Dictionary<string, string> postedFormDictionary = new Dictionary<string, string>();

            foreach (KeyValuePair<string, object> pair in SubmissionData.Data)
            {
                if (!pair.Key.ToLower().StartsWith("systemcolumn") && pair.Value != null)
                {
                    postedFormDictionary.Add(pair.Key, pair.Value.ToString());
                }
            }

            var mappings = base.ActiveExternalFieldMappingTable;
            if (mappings != null)
            {
                // Attributes used to be sent to the members profile
                Dictionary<string, string> formAttributes = new Dictionary<string, string>();

                foreach (var item in mappings)
                {
                    if (item.Value != null)
                    {
                        var fieldName = item.Key;
                        var remoteFieldName = item.Value.ColumnId;

                        if (postedFormDictionary.ContainsKey(fieldName))
                        {
                            formAttributes.Add(remoteFieldName, postedFormDictionary[fieldName]);

                            if (remoteFieldName.Equals("email", System.StringComparison.OrdinalIgnoreCase))
                            {
                                email = postedFormDictionary[fieldName];
                            }
                        }
                    }
                }

                if (formAttributes.Any() && !string.IsNullOrWhiteSpace(email))
                {
                    validProfileSave = _odpService.Service.SaveProfileInformation(email, formAttributes);
                }

                if (validProfileSave)
                {
                    bool consentGiven = false;
                    var currentForm = ContentLoader.Service.Get<IContent>(FormIdentity.Guid);

                    if (currentForm != null && currentForm is FormContainerBlock formContainerBlock)
                    {
                        if (formContainerBlock.ElementsArea.Items.Any())
                        {
                            _formContents = formContainerBlock.ElementsArea.Items.Select(x => ContentLoader.Service.Get<IContent>(x.ContentLink));

                            // Try to get the list the editor would like the user to be added too.
                            var listIds = TryGetListIds();

                            // Check to see if user has consented to add to a list if selected
                            if (listIds.Any())
                            {
                                consentGiven = TryGetUserConsent();
                            }

                            if (listIds.Any() && !string.IsNullOrWhiteSpace(email))
                            {
                                foreach (var listId in listIds)
                                {
                                    var listSubscription = new ODPListSubscribeRequest()
                                    {
                                        ListId = listId,
                                        Email = email,
                                        Subscribed = consentGiven  // Will add to list but without consent if set to false
                                    };
                                    _odpService.Service.AddToList(listSubscription);
                                }
                            }
                        }
                    }
                }
            }

            return str;
        }

        private IEnumerable<string> TryGetListIds()
        {
            List<string> listIds = new List<string>();
            var odpListFormBlock = _formContents.FirstOrDefault(x => x is ODPListFormBlock);
            if (odpListFormBlock != null && odpListFormBlock is ODPListFormBlock listFormBlock)
            {
                var ids = listFormBlock.ListId;
                if (!string.IsNullOrWhiteSpace(ids))
                {
                    listIds = ids.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries).ToList();
                }
            }
            return listIds;
        }

        private bool TryGetUserConsent()
        {
            bool consentGiven = false;
            var listConsent = _formContents.FirstOrDefault(x => x is ODPListConsentFormBlock);
            if (listConsent != null)
            {
                var id = $"__field_{listConsent.ContentLink.ToReferenceWithoutVersion().ID}";
                if (SubmissionData.Data.ContainsKey(id))
                {
                    var consentStringValue = SubmissionData.Data[id].ToString();
                    if (bool.TryParse(SubmissionData.Data[id].ToString(), out bool consentGivenValue))
                    {
                        consentGiven = consentGivenValue;
                    }
                    else if (consentStringValue.Equals("Yes", System.StringComparison.OrdinalIgnoreCase) || consentStringValue.Equals("y", System.StringComparison.OrdinalIgnoreCase) || consentStringValue.Equals("on", System.StringComparison.OrdinalIgnoreCase))
                    {
                        consentGiven = true;
                    }
                }
            }
            return consentGiven;
        }

        public string ObjectType
        {
            get
            {
                return _objectType;
            }

            set
            {
                _objectType = value;
            }
        }
    }
}