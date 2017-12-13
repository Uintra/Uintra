using System;
using System.Collections.Generic;
using uIntra.CentralFeed;
using uIntra.Core.Activity;
using uIntra.Core.Extensions;
using uIntra.Core.PagePromotion;
using uIntra.Core.TypeProviders;

namespace Compent.uIntra.Core.PagePromotion
{
    public class PagePromotionService : IPagePromotionService<PagePromotion>, IFeedItemService
    {
        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly IFeedTypeProvider _feedTypeProvider;

        public PagePromotionService(IActivityTypeProvider activityTypeProvider, IFeedTypeProvider feedTypeProvider)
        {
            _activityTypeProvider = activityTypeProvider;
            _feedTypeProvider = feedTypeProvider;
        }

        public IIntranetType ActivityType => _activityTypeProvider.Get(IntranetActivityTypeEnum.PagePromotion.ToInt());

        public FeedSettings GetFeedSettings()
        {
            return new FeedSettings
            {
                Type = _feedTypeProvider.Get(CentralFeedTypeEnum.PagePromotion.ToInt()),
                Controller = "PagePromotion",
                HasSubscribersFilter = false,
                HasPinnedFilter = false
            };
        }

        public PagePromotion GetPagePromotion(Guid pageId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IFeedItem> GetItems()
        {
            throw new NotImplementedException();
        }
    }
}