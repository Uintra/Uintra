using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Compent.Extensions;
using FluentScheduler;
using Uintra20.Core.Jobs.Configuration;
using Uintra20.Core.Jobs.Enums;
using Uintra20.Core.Jobs.Models;
using Uintra20.Infrastructure.Extensions;
using Umbraco.Core.Composing;

namespace Uintra20.Core.Jobs
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
			var types = Current.Factory.EnsureScope(s => (IEnumerable<IJob>)s.GetAllInstances(typeof(Uintra20BaseIntranetJob)));

			foreach (var type in types)
			{
				var jobConfiguration = GetConfiguration(type.GetType().Name);

				ValidateConfiguration(jobConfiguration);

				if (jobConfiguration.Enabled)
				{
					var schedule = AddJob(type);

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
			return Schedule(() => job);
		}

		private JobConfiguration GetConfiguration(string jobName)
		{
			var jobSettings = _jobSettingsConfiguration.Settings.FirstOrDefault(s => s.Job.Equals(jobName, StringComparison.OrdinalIgnoreCase));

			if (jobSettings == null)
			{
				return new JobConfiguration() { Enabled = false };
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

		private void ValidateConfiguration(JobConfiguration configuration)
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
