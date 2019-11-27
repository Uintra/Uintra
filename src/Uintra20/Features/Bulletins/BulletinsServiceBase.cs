using Uintra20.Core.Activity;
using Uintra20.Core.Member;
using Uintra20.Core.Member.Entities;
using Uintra20.Features.LinkPreview;
using Uintra20.Features.Location.Services;
using Uintra20.Features.Media;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Infrastructure.Caching;
using Uintra20.Infrastructure.TypeProviders;

namespace Uintra20.Features.Bulletins
{
    public abstract class BulletinsServiceBase<TBulletin> : IntranetActivityService<TBulletin> where TBulletin : BulletinBase
    {
        protected BulletinsServiceBase(
            IIntranetActivityRepository activityRepository,
            ICacheService cache,
            IActivityTypeProvider activityTypeProvider,
            IIntranetMediaService intranetMediaService,
            IActivityLocationService activityLocationService,
            IActivityLinkPreviewService activityLinkPreviewService,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IPermissionsService permissionsService
        )
            : base(activityRepository, cache, activityTypeProvider, intranetMediaService, activityLocationService, activityLinkPreviewService, intranetMemberService, permissionsService)
        {
        }
    }
}