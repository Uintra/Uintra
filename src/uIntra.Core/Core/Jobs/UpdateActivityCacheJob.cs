using System.Collections.Generic;
using uIntra.Core.Activity;

namespace uIntra.Core.Jobs
{
    public class UpdateActivityCacheJob : BaseIntranetJob
    {
        private readonly IEnumerable<IIntranetActivityService<IIntranetActivity>> _activityServices;

        public UpdateActivityCacheJob(IEnumerable<IIntranetActivityService<IIntranetActivity>> activityServices)
        {
            _activityServices = activityServices;
        }

        public override JobSettings GetSettings()
        {
            return new JobSettings()
            {
                RunType = JobRunTypeEnum.RunEvery,
                TimeType = JobTimeType.Minutes,
                IsEnabled = true,
                Time = 1
            };
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
                        service.Save(activity);
                    }
                }
            }

        }                   
    }
}