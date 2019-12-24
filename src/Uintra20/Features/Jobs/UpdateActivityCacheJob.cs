using System.Collections.Generic;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Entities;
using Uintra20.Core.Jobs.Models;

namespace Uintra20.Features.Jobs
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