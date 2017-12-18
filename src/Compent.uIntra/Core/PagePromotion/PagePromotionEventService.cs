using System.Linq;
using System.Web.Mvc;
using uIntra.Core.PagePromotion;
using uIntra.Core.UmbracoEventServices;
using uIntra.Search;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;
using Umbraco.Core.Services;

namespace Compent.uIntra.Core.PagePromotion
{
    public class PagePromotionEventService : IUmbracoEventService<IPublishingStrategy, PublishEventArgs<IContent>>
    {
        public void Process(IPublishingStrategy sender, PublishEventArgs<IContent> publishEventArgs)
        {
            foreach (var entity in publishEventArgs.PublishedEntities)
            {
                UpdatePagePromotionFilesCache(entity);
            }
        }

        private static void UpdatePagePromotionFilesCache(IContent content)
        {
            if (!PagePromotionHelper.IsPagePromotion(content)) return;

            var contentService = DependencyResolver.Current.GetService<IContentService>();
            var documentIndexer = DependencyResolver.Current.GetService<IDocumentIndexer>();

            var newsFilesIds = PagePromotionHelper.GetFileIds(content).ToList();

            if (content.Published)
            {
                var newContentConfig = PagePromotionHelper.GetConfig(content);
                if (!newContentConfig.PromoteOnCentralFeed)
                {
                    documentIndexer.DeleteFromIndex(newsFilesIds);
                    return;
                }

                var oldContentVersion = contentService.GetVersions(content.Id).FirstOrDefault(c => c.Version != content.Version);
                var oldFilesIds = PagePromotionHelper.GetFileIds(oldContentVersion).ToList();
              
                documentIndexer.DeleteFromIndex(oldFilesIds.Except(newsFilesIds));
                documentIndexer.Index(newsFilesIds);
            }
            else
            {
                documentIndexer.DeleteFromIndex(newsFilesIds);
            }
        }
    }
}
