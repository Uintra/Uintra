using System;
using Uintra.Core.Activity;
using Uintra.Core.Activity.Entities;
using Uintra.Core.Member.Entities;
using Uintra.Core.Member.Services;
using Uintra.Features.Groups.Services;
using Uintra.Features.LinkPreview.Services;
using Uintra.Features.Location.Services;
using Uintra.Features.Media.Intranet.Services.Contracts;
using Uintra.Features.Permissions.Interfaces;
using Uintra.Infrastructure.Caching;
using Uintra.Infrastructure.Extensions;
using Uintra.Infrastructure.TypeProviders;

namespace Uintra.Features.News
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
            IPermissionsService permissionsService,
            IGroupActivityService groupActivityService,
            IGroupService groupService)
            : base(activityRepository, cache, activityTypeProvider, intranetMediaService, activityLocationService, activityLinkPreviewService,
                intranetMemberService, permissionsService, groupActivityService, groupService)
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