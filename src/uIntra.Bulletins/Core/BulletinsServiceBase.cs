using Uintra.Core.Activity;
using Uintra.Core.Caching;
using Uintra.Core.LinkPreview;
using Uintra.Core.Location;
using Uintra.Core.Media;
using Uintra.Core.TypeProviders;

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
            IActivityLinkPreviewService activityLinkPreviewService)
            : base(activityRepository, cache, activityTypeProvider, intranetMediaService, activityLocationService, activityLinkPreviewService)
        {
        }
    }
}
