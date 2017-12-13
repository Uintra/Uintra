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
    class SearchContentEventService : IUmbracoEventService<IPublishingStrategy, PublishEventArgs<IContent>>
    {

        public void Process(IPublishingStrategy sender, PublishEventArgs<IContent> publishEventArgs)
        {
            var contentIndexer = DependencyResolver.Current.GetService<IContentIndexer>();
            var umbracoHelper = DependencyResolver.Current.GetService<UmbracoHelper>();

            foreach (var entity in publishEventArgs.PublishedEntities)
            {
                contentIndexer.FillIndex(entity.Id);
                if (IsGlobalPanel(entity))
                    umbracoHelper.TypedContentAtRoot()
                        .SelectMany(c => c.DescendantsOrSelf())
                        .Where(c => ContainsGlobalPanel(c, entity))
                        .Select(c => c.Id)
                        .ToList()
                        .ForEach(contentIndexer.FillIndex);
            }
        }

        private static bool IsGlobalPanel(IContent entity) =>
            entity.Parent()?.ContentType.Alias == DocumentTypeAliasConstants.GlobalPanelFolder;

        static IGridHelper gridHelper = DependencyResolver.Current.GetService<IGridHelper>();
        private static bool ContainsGlobalPanel(IPublishedContent content, IContent globalPanel)
        {
            return gridHelper
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
