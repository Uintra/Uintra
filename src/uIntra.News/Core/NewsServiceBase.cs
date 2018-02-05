using System;
using uIntra.Core;
using uIntra.Core.Activity;
using uIntra.Core.Caching;
using uIntra.Core.Location;
using uIntra.Core.Media;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;

namespace uIntra.News
{
    public abstract class NewsServiceBase<TNews> : IntranetActivityService<TNews> where TNews : NewsBase
    {
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        protected NewsServiceBase(IIntranetActivityRepository activityRepository,
            ICacheService cache,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IActivityTypeProvider activityTypeProvider,
            IIntranetMediaService intranetMediaService,
            IActivityLocationService activityLocationService)
            : base(activityRepository, cache, activityTypeProvider, intranetMediaService, activityLocationService)
        {
            _intranetUserService = intranetUserService;
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
            var owner = _intranetUserService.Get(newsEntity);
            var currentUserId = _intranetUserService.GetCurrentUserId();

            if (owner.Id != currentUserId)
            {
                return IsExpired(newsEntity);
            }

            return false;
        }
    }
}