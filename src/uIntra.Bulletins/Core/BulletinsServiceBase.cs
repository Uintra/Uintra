using Uintra.Core.Activity;
using Uintra.Core.Caching;
using Uintra.Core.LinkPreview;
using Uintra.Core.Location;
using Uintra.Core.Media;
using Uintra.Core.Permissions.Interfaces;
using Uintra.Core.TypeProviders;
using Uintra.Core.User;

namespace Uintra.Bulletins
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
