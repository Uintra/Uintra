using Uintra20.Core.Activity;
using Uintra20.Core.Caching;
using Uintra20.Core.LinkPreview;
using Uintra20.Core.Location;
using Uintra20.Core.Media;
using Uintra20.Core.Permissions.Interfaces;
using Uintra20.Core.TypeProviders;
using Uintra20.Core.User;

namespace Uintra20.Core.Bulletins
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
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            IPermissionsService permissionsService
        )
            : base(activityRepository, cache, activityTypeProvider, intranetMediaService, activityLocationService, activityLinkPreviewService, intranetMemberService, permissionsService)
        {
        }
    }
}