using System.Web.Mvc;
using uIntra.Search.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;

namespace Compent.uIntra.Core
{
    public static class UmbracoEventsModule
    {
        private static IContentIndexer _contentIndexer;

        public static void RegisterEvents()
        {
            Init();
            Umbraco.Core.Services.ContentService.Published += ContentServiceOnPublished;
            Umbraco.Core.Services.ContentService.UnPublished += ContentServiceOnUnPublished;
        }

        private static void Init()
        {
            _contentIndexer = DependencyResolver.Current.GetService<IContentIndexer>();
        }

        private static void ContentServiceOnPublished(IPublishingStrategy sender, PublishEventArgs<IContent> publishEventArgs)
        {
            foreach (var entity in publishEventArgs.PublishedEntities)
            {
                _contentIndexer.FillIndex(entity.Id);
            }
        }

        private static void ContentServiceOnUnPublished(IPublishingStrategy sender, PublishEventArgs<IContent> publishEventArgs)
        {
            foreach (var entity in publishEventArgs.PublishedEntities)
            {
                _contentIndexer.DeleteFromIndex(entity.Id);
            }
        }
    }
}