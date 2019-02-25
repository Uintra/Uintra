using System;
using Uintra.Core.Activity;
using Uintra.Core.Caching;
using Uintra.Core.Extensions;
using Uintra.Core.LinkPreview;
using Uintra.Core.Location;
using Uintra.Core.Media;
using Uintra.Core.Permissions;
using Uintra.Core.TypeProviders;
using Uintra.Core.User;

namespace Uintra.News
{
    public abstract class NewsServiceBase<TNews> : IntranetActivityService<TNews> where TNews : NewsBase
    {
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;

        protected NewsServiceBase(
            IIntranetActivityRepository activityRepository,
            ICacheService cache,
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            IActivityTypeProvider activityTypeProvider,
            IIntranetMediaService intranetMediaService,
            IActivityLocationService activityLocationService,
            IActivityLinkPreviewService activityLinkPreviewService,
            IPermissionsService permissionsService)
            : base(activityRepository, cache, activityTypeProvider, intranetMediaService, activityLocationService, activityLinkPreviewService,
                  intranetMemberService, permissionsService)
        {
            _intranetMemberService = intranetMemberService;
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
            var owner = _intranetMemberService.Get(newsEntity);
            var currentMemberId = _intranetMemberService.GetCurrentMemberId();
            return owner.Id == currentMemberId;
        }
    }
}