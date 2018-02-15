using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Extensions;
using FluentScheduler;
using Uintra.Core.Extensions;
using Uintra.Core.Jobs.Configuration;
using Uintra.Core.Jobs.Enums;
using Uintra.Core.Jobs.Models;

namespace Uintra.Core.Jobs
{
    public class JobsRegistry : Registry
    {
        private readonly IJobSettingsConfiguration _jobSettingsConfiguration;

        public JobsRegistry()
        {
            _jobSettingsConfiguration = DependencyResolver.Current.GetService<IJobSettingsConfiguration>();
            SetupJobs();
        }

        public void SetupJobs()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var types = assemblies.Select(assembly => assembly.GetTypes().Where(s => typeof(BaseIntranetJob).IsAssignableFrom(s) && s != typeof(BaseIntranetJob)))
                .SelectMany(t => t);

            foreach (var type in types)
            {
                dynamic baseJob = DependencyResolver.Current.GetService(type);

                JobConfiguration jobConfiguration = GetConfiguration(baseJob.GetType().Name);

                VailidateConfiguration(jobConfiguration);

                if (jobConfiguration.Enabled)
                {
                    var schedule = AddJob(baseJob);

                    if (jobConfiguration.RunType == JobRunTypeEnum.RunOnceAtDate)
                    {
                        schedule.ToRunOnceAt(jobConfiguration.Date.GetValueOrDefault());
                        continue;
                    }

                    var timeUnit = ResolveRunType(schedule, jobConfiguration);
                    ResolveTimeType(timeUnit, jobConfiguration);
                }
            }
        }

        private Schedule AddJob<T>(T job) where T : IJob
        {
            return Schedule<T>();
        }

        private JobConfiguration GetConfiguration(string jobName)
        {
            var jobSettings = _jobSettingsConfiguration.Settings.FirstOrDefault(s => s.Job.Equals(jobName, StringComparison.OrdinalIgnoreCase));

            if (jobSettings == null)
            {
                return new JobConfiguration() { Enabled = true };
            }

            return jobSettings.Map<JobConfiguration>();
        }

        private TimeUnit ResolveRunType(Schedule schedule, JobConfiguration configuration)
        {
            var time = configuration.Time.GetValueOrDefault();

            switch (configuration.RunType)
            {
                case JobRunTypeEnum.RunEvery:
                    return schedule.ToRunEvery(time);
                case JobRunTypeEnum.RunNow:
                    return schedule.ToRunNow().AndEvery(time);
                case JobRunTypeEnum.RunOnceIn:
                    return schedule.ToRunOnceIn(time);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }



        private void ResolveTimeType(TimeUnit timeUnit, JobConfiguration configuration)
        {
            switch (configuration.TimeType)
            {
                case JobTimeTypeEnum.Seconds:
                    timeUnit.Seconds();
                    break;
                case JobTimeTypeEnum.Minutes:
                    timeUnit.Minutes();
                    break;
                case JobTimeTypeEnum.Hours:
                    timeUnit.Hours();
                    break;
                case JobTimeTypeEnum.Days:
                    var dayUnit = timeUnit.Days();
                    if (configuration.AtHour.HasValue && configuration.AtMinutes.HasValue)
                    {
                        dayUnit.At(configuration.AtHour.Value, configuration.AtMinutes.Value);
                    }
                    break;
                default:
                    throw new Exception($"Unexpected job time type - {configuration.TimeType}");
            }


        }

        private void VailidateConfiguration(JobConfiguration configuration)
        {
            if (configuration.RunType.In(JobRunTypeEnum.RunEvery, JobRunTypeEnum.RunOnceIn) && !configuration.Time.HasValue)
            {
                throw new Exception($"Job with run type - {configuration.RunType} should have time value");
            }

            if (configuration.RunType == JobRunTypeEnum.RunOnceAtDate && !configuration.Date.HasValue)
            {
                throw new Exception($"Job with run type - {configuration.RunType} should have date value");
            }
        }
    }
}
