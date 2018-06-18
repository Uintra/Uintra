using System.Collections.Generic;
using Uintra.Core.Activity;
using Uintra.Core.Jobs.Models;

namespace Uintra.Core.Jobs
{
    public class UpdateActivityCacheJob : BaseIntranetJob
    {
        private readonly IEnumerable<IIntranetActivityService<IIntranetActivity>> _activityServices;

        public UpdateActivityCacheJob(IEnumerable<IIntranetActivityService<IIntranetActivity>> activityServices)
        {
            _activityServices = activityServices;            
        }       

        public override void Action()
        {
            ProcessActivities();
        }

        private void ProcessActivities()
        {
            foreach (var service in _activityServices)
            {
                var intranetActivities = service.GetAll();

                foreach (var activity in intranetActivities)
                {
                    if (activity.IsPinActual && !service.IsPinActual(activity))
                    {
                        var cacheableIntranetActivityService = (ICacheableIntranetActivityService<IIntranetActivity>)service;
                        cacheableIntranetActivityService.UpdateActivityCache(activity.Id);
                    }
                }
            }

        }                   
    }
}