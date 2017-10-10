using uIntra.Core.Activity;
using uIntra.Core.Caching;
using uIntra.Core.Media;
using uIntra.Core.TypeProviders;

namespace uIntra.Bulletins
{
    public abstract class BulletinsServiceBase<TBulletin> : IntranetActivityService<TBulletin> where TBulletin : BulletinBase
    {
        protected BulletinsServiceBase(
            IIntranetActivityRepository activityRepository,
            ICacheService cache,
            IActivityTypeProvider activityTypeProvider,
            IIntranetMediaService intranetMediaService) 
            : base(activityRepository, cache, activityTypeProvider, intranetMediaService)
        {
        }
    }
}
