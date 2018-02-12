using System.Web.Mvc;
using Uintra.Core.PagePromotion;
using Uintra.Core.UmbracoEventServices;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;

namespace Compent.Uintra.Core.PagePromotion
{
    public class PagePromotionEventService : IUmbracoContentPublishedEventService
    {
        public void ProcessContentPublished(IPublishingStrategy sender, PublishEventArgs<IContent> publishEventArgs)
        {
            foreach (var entity in publishEventArgs.PublishedEntities)
            {
                UpdatePagePromotionCache(entity);
            }
        }

        private static void UpdatePagePromotionCache(IContent content)
        {
            if (!PagePromotionHelper.IsPagePromotion(content)) return;

            var pagePromotionService = DependencyResolver.Current.GetService<IPagePromotionService<Entities.PagePromotion>>();

            if (content.Published)
            {
                pagePromotionService.Save(new Entities.PagePromotion { Id = content.Key });
            }
            else
            {
                pagePromotionService.Delete(content.Key);
            }
        }
    }
}
