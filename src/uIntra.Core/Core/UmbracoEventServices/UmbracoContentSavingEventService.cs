using System.Linq;
using Compent.Extensions;
using Newtonsoft.Json.Linq;
using Uintra.Core.Constants;
using Uintra.Core.Core.UmbracoEventServices;
using Uintra.Core.Grid;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

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
            var contentPanels = args.SavedEntities
                .SelectMany(content => _gridHelper
                    .GetValues(content, GridEditorConstants.ContentPanelAlias)
                    .Select(x => x.value));

            var result = contentPanels.All(IsValidContentPanel);

            if (!result)
            {
                var message = new EventMessage("Content panel error", "empty content", EventMessageType.Error);
                args.CancelOperation(message);
            }
        }

        private static bool IsValidContentPanel(dynamic contentPanel)
        {
            var contentPanelObject = contentPanel as JObject;
            var contentPanelTitle = contentPanelObject?["title"]?.Value<string>();
            return contentPanelObject != null && contentPanelTitle.HasValue();
        }
    }
}
