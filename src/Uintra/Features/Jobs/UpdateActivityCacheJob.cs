using System.Collections.Generic;
using Uintra.Core.Activity;
using Uintra.Core.Activity.Entities;
using Uintra.Core.Jobs.Models;

namespace Uintra.Features.Jobs
{
    public class UpdateActivityCacheJob : UintraBaseIntranetJob
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