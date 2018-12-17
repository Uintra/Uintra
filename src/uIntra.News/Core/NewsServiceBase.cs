using System;
using Uintra.Core.Activity;
using Uintra.Core.Caching;
using Uintra.Core.Extensions;
using Uintra.Core.LinkPreview;
using Uintra.Core.Location;
using Uintra.Core.Media;
using Uintra.Core.TypeProviders;
using Uintra.Core.User;

namespace Uintra.News
{
    public abstract class NewsServiceBase<TNews> : IntranetActivityService<TNews> where TNews : NewsBase
    {
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        protected NewsServiceBase(
            IIntranetActivityRepository activityRepository,
            ICacheService cache,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IActivityTypeProvider activityTypeProvider,
            IIntranetMediaService intranetMediaService,
            IActivityLocationService activityLocationService,
            IActivityLinkPreviewService activityLinkPreviewService)
            : base(activityRepository, cache, activityTypeProvider, intranetMediaService, activityLocationService, activityLinkPreviewService)
        {
            _intranetUserService = intranetUserService;
        }

        public override bool IsActual(IIntranetActivity activity)
        {
            var news = (NewsBase)activity;
            var isActual = base.IsActual(news);

            if (!isActual) return false;

            if (IsExpired(news))
            {
                news.IsHidden = true;
                news.UnpublishDate = null;
                Save(news);
                return false;
            }

            return news.PublishDate <= DateTime.UtcNow || IsOwner(news);
        }

        public virtual bool IsExpired(INewsBase news)
        {
            return news.UnpublishDate.HasValue && news.UnpublishDate.Value < DateTime.UtcNow;
        }

        protected virtual bool IsOwner(NewsBase newsEntity)
        {
            var owner = _intranetUserService.Get(newsEntity);
            var currentUserId = _intranetUserService.GetCurrentUserId();
            return owner.Id == currentUserId;
        }
    }
}