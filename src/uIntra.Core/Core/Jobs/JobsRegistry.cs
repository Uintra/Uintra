using System;
using System.Web.Mvc;
using FluentScheduler;
using uIntra.Core.Extensions;

namespace uIntra.Core.Jobs
{
    public class JobsRegistry : Registry
    {
        public JobsRegistry()
        {
            SetupJobs();
        }

        public void SetupJobs()
        {
            var baseIntranetJobs = DependencyResolver.Current.GetServices<BaseIntranetJob>();

            foreach (var baseJob in baseIntranetJobs)
            {
                var jobSettings = baseJob.GetSettings();

                VailidateSettings(jobSettings);

                if (jobSettings.IsEnabled)
                {
                    var schedule = Schedule(() =>
                    {
                        baseJob.Action();
                    });

                    if (jobSettings.RunType == JobRunTypeEnum.RunOnceAtDate)
                    {
                        schedule.ToRunOnceAt(jobSettings.Date.GetValueOrDefault());
                        continue;                        
                    }

                    var timeUnit = ResolveRunType(schedule, jobSettings);
                    ResolveTimeType(timeUnit, jobSettings);
                }
            }
        }

        private TimeUnit ResolveRunType(Schedule schedule, JobSettings settings)
        {
            var time = settings.Time.GetValueOrDefault();

            switch (settings.RunType)
            {
                case JobRunTypeEnum.RunEvery:
                    return schedule.ToRunEvery(time);
                case JobRunTypeEnum.RunNow:
                    return schedule.ToRunNow().AndEvery(time);
                case JobRunTypeEnum.RunOnceAt:
                    return schedule.ToRunOnceIn(time);
                default:
                    throw new Exception($"Unexpected job run type - {settings.RunType}");
            }
        }



        private void ResolveTimeType(TimeUnit timeUnit, JobSettings settings)
        {
            switch (settings.TimeType)
            {
                case JobTimeType.Seconds:
                    timeUnit.Seconds();
                    break;
                case JobTimeType.Minutes:
                    timeUnit.Minutes();
                    break;
                case JobTimeType.Hours:
                    timeUnit.Hours();
                    break;
                case JobTimeType.Days:
                    timeUnit.Days();
                    break;
                default:
                    throw new Exception($"Unexpected job time type - {settings.TimeType}");
            }
        }

        private void VailidateSettings(JobSettings settings)
        {
            if (settings.RunType.In(JobRunTypeEnum.RunEvery, JobRunTypeEnum.RunNow, JobRunTypeEnum.RunOnceAt) && !settings.Time.HasValue)
            {
                throw new Exception($"Job with run type - {settings.RunType} should have time value");
            }

            if (settings.RunType == JobRunTypeEnum.RunOnceAtDate && !settings.Date.HasValue)
            {
                throw new Exception($"Job with run type - {settings.RunType} should have date value");
            }
        }

    }
}
