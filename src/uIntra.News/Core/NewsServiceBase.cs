using System;
using uIntra.Core;
using uIntra.Core.Activity;
using uIntra.Core.Caching;
using uIntra.Core.Extentions;
using uIntra.Core.Media;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;

namespace uIntra.News
{
    public abstract class NewsServiceBase<TNews> : IntranetActivityService<TNews> where TNews : NewsBase
    {
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IIntranetUserContentHelper _intranetUserContentHelper;

        protected NewsServiceBase(IIntranetActivityRepository activityRepository,
            ICacheService cache,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IActivityTypeProvider activityTypeProvider,
            IIntranetMediaService intranetMediaService,
            IIntranetUserContentHelper intranetUserContentHelper)
            : base(activityRepository, cache, activityTypeProvider, intranetMediaService, intranetUserService, intranetUserContentHelper)
        {
            _intranetUserService = intranetUserService;
            _intranetUserContentHelper = intranetUserContentHelper;
        }

        public override bool IsActual(IIntranetActivity cachedActivity)
        {
            var activity = (NewsBase)cachedActivity;
            return base.IsActual(activity) && activity.PublishDate.Date <= DateTime.Now.Date && !IsShowIfUnpublish(activity);
        }

        public virtual bool IsExpired(INewsBase news)
        {
            return news.UnpublishDate.HasValue && news.UnpublishDate.Value.Date < DateTime.Now;
        }

        protected virtual bool IsShowIfUnpublish(NewsBase newsEntity)
        {
            var creator = _intranetUserService.Get(newsEntity);
            var currentUserId = _intranetUserService.GetCurrentUserId();

            if (creator.Id != currentUserId)
            {
                return IsExpired(newsEntity);
            }

            return false;
        }
    }
}