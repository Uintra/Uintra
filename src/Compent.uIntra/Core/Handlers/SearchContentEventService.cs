using System.Linq;
using System.Web.Mvc;
using Compent.uIntra.Core.Constants;
using Newtonsoft.Json.Linq;
using uIntra.Core.Constants;
using uIntra.Core.Grid;
using uIntra.Core.UmbracoEventServices;
using uIntra.Search;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;
using Umbraco.Web;

namespace Compent.uIntra.Core.Handlers
{
    public class SearchContentEventService : IUmbracoContentUnPublishedEventService, IUmbracoContentPublishedEventService
    {
        private readonly IContentIndexer _contentIndexer;
        private readonly IGridHelper _gridHelper;
        private readonly UmbracoHelper _umbracoHelper;

        public SearchContentEventService(IContentIndexer contentIndexer, IGridHelper gridHelper, UmbracoHelper umbracoHelper)
        {
            _contentIndexer = contentIndexer;
            _gridHelper = gridHelper;
            _umbracoHelper = umbracoHelper;
        }

        public void ProcessContentPublished(IPublishingStrategy sender, PublishEventArgs<IContent> args)
        {
            foreach (var entity in args.PublishedEntities)
            {
                _contentIndexer.FillIndex(entity.Id);
                if (IsGlobalPanel(entity))
                    _umbracoHelper.TypedContentAtRoot()
                        .SelectMany(c => c.DescendantsOrSelf())
                        .Where(c => ContainsGlobalPanel(c, entity))
                        .Select(c => c.Id)
                        .ToList()
                        .ForEach(_contentIndexer.FillIndex);
            }
        }

        public void ProcessContentUnPublished(IPublishingStrategy sender, PublishEventArgs<IContent> args)
        {
            foreach (var entity in args.PublishedEntities) _contentIndexer.DeleteFromIndex(entity.Id);
        }

        private static bool IsGlobalPanel(IContent entity) =>
            entity.Parent()?.ContentType.Alias == DocumentTypeAliasConstants.GlobalPanelFolder;

        private bool ContainsGlobalPanel(IPublishedContent content, IContent globalPanel)
        {
            return _gridHelper
                .GetValues(content, GridEditorConstants.GlobalPanelPickerAlias)
                .Any(t => ContainsGlobalPanel(t.value, globalPanel));
        }

        private static bool ContainsGlobalPanel(dynamic panel, IContent globalPanel)
        {
            int? id = GetPanelId(panel);
            return id == globalPanel.Id;
        }

        private static int? GetPanelId(object panel) =>
            panel is JObject json
                ? json["id"].ToObject<int?>()
                : default;
    }
}
