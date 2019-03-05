using System.Linq;
using Compent.Extensions;
using LanguageExt;
using Newtonsoft.Json.Linq;
using Uintra.Core.Constants;
using Uintra.Core.Core.UmbracoEventServices;
using Uintra.Core.Grid;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using static LanguageExt.Prelude;

namespace Uintra.Core.UmbracoEventServices
{
    public class UmbracoContentSavingEventService: IUmbracoContentSavingEventService
    {
        private readonly IGridHelper _gridHelper;

        public UmbracoContentSavingEventService(IGridHelper gridHelper)
        {
            _gridHelper = gridHelper;
        }


        public void ProcessContentSaving(IContentService sender, SaveEventArgs<IContent> args)
        {
            var panels = args.SavedEntities
                .SelectMany(content => _gridHelper
                    .GetValues(content, 
                        GridEditorConstants.ContentPanelAlias,
                        GridEditorConstants.DocumentLibraryPanelAlias,
                        GridEditorConstants.FaqPanelAlias,
                        GridEditorConstants.GlobalPanelPickerAlias));

            var validationResult = panels
                .Select<(string alias, dynamic value), Validation<string, Unit>>(panel => IsValidPanel(panel.alias, panel.value))
                .Sequence();

            validationResult.IfFail(errors => errors
                .Select(error => new EventMessage("Panel error", error, EventMessageType.Error))
                .Iter(args.CancelOperation));
        }


        private static Validation<string, Unit> IsValidPanel(string gridEditorAlias, dynamic panel)
        {
            switch (gridEditorAlias)
            {
                case GridEditorConstants.ContentPanelAlias:
                    return IsValidContentPanel(panel);
                case GridEditorConstants.DocumentLibraryPanelAlias:
                    return IsValidDocumentPanel(panel);
                case GridEditorConstants.FaqPanelAlias:
                    return IsValidFaqPanel(panel);
                case GridEditorConstants.GlobalPanelPickerAlias:
                    return IsValidGlobalPanel(panel);
                default:
                    return unit;
            }
        }

        private static Validation<string, Unit> IsValidContentPanel(dynamic contentPanel)
        {
            var contentPanelObject = contentPanel as JObject;
            var contentPanelTitle = contentPanelObject?["title"]?.Value<string>();

            if (contentPanelObject != null && contentPanelTitle.HasValue())
            {
                return unit;
            }
            else
            {
                return "Empty content panel";
            }
        }

        private static Validation<string, Unit> IsValidDocumentPanel(dynamic documentPanel)
        {
            var documentPanelObject = documentPanel as JObject;
            var documentPanelTitle = documentPanelObject?["title"]?.Value<string>();

            if (documentPanelObject != null && documentPanelTitle.HasValue())
            {
                return unit;
            }
            else
            {
                return "Empty document panel";
            }
        }

        private static Validation<string, Unit> IsValidFaqPanel(dynamic faqPanel)
        {
            var faqPanelObject = faqPanel as JObject;
            var faqPanelTitle = faqPanelObject?["title"]?.Value<string>();

            if (faqPanelObject != null && faqPanelTitle.HasValue())
            {
                return unit;
            }
            else
            {
                return "Empty FAQ panel";
            }
        }

        private static Validation<string, Unit> IsValidGlobalPanel(dynamic faqPanel)
        {
            var faqPanelObject = faqPanel as JObject;
            var faqPanelTitle = faqPanelObject?["id"]?.Value<string>();

            if (faqPanelObject != null && faqPanelTitle.HasValue())
            {
                return unit;
            }
            else
            {
                return "Empty global panel";
            }
        }
    }
}
