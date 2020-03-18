using System;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Entities;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.LinkPreview;
using Uintra20.Features.Location.Services;
using Uintra20.Features.Media;
using Uintra20.Features.Media.Intranet.Services.Contracts;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Infrastructure.Caching;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.TypeProviders;

namespace Uintra20.Features.News
{
    public abstract class NewsServiceBase<TNews> : IntranetActivityService<TNews> where TNews : NewsBase
    {
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;

        protected NewsServiceBase(
            IIntranetActivityRepository activityRepository,
            ICacheService cache,
            IIntranetMemberService<IntranetMember> intranetMemberService,
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