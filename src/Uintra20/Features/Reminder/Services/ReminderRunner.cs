using System;
using System.Linq;
using Uintra20.Core.Activity;
using Uintra20.Core.Configuration;
using Uintra20.Features.Notification.Configuration;
using Uintra20.Features.Notification.Configuration.BackofficeSettings.Providers;
using Uintra20.Features.Notification.Entities.Base;
using Uintra20.Features.Notification.Services;
using Uintra20.Features.Reminder.Configuration;
using Uintra20.Infrastructure.Exceptions;

namespace Uintra20.Features.Reminder.Services
{
	public class ReminderRunner : IReminderRunner
	{
		private readonly IReminderService _reminderService;
		private readonly IActivitiesServiceFactory _activitiesServiceFactory;
		private readonly IConfigurationProvider<ReminderConfiguration> _configurationProvider;
		private readonly IExceptionLogger _exceptionLogger;
		private readonly INotificationTypeProvider _notificationTypeProvider;
		private static bool _isRunning;

		public ReminderRunner(
			IReminderService reminderService,
			IActivitiesServiceFactory activitiesServiceFactory,
			IConfigurationProvider<ReminderConfiguration> configurationProvider,
			IExceptionLogger exceptionLogger,
			INotificationTypeProvider notificationTypeProvider)
		{
			_reminderService = reminderService;
			_activitiesServiceFactory = activitiesServiceFactory;
			_configurationProvider = configurationProvider;
			_exceptionLogger = exceptionLogger;
			_notificationTypeProvider = notificationTypeProvider;
		}

		public void Run()
		{
			if (_isRunning)
			{
				return;
			}
			_isRunning = true;

			try
			{
				var reminders = _reminderService.GetAllNotDelivered();
				foreach (var reminder in reminders)
				{
					var service = _activitiesServiceFactory.GetService<IIntranetActivityService>(reminder.ActivityId);
					var reminderService = service as IReminderableService<IReminderable>;
					var notifiableService = service as INotifyableService;

					if (reminderService == null || notifiableService == null)
					{
						continue;
					}

					var activity = reminderService.GetActual(reminder.ActivityId);
					if (activity != null)
					{
						var configuration = GetConfiguration(reminder.Type);
						if (ShouldNotify(configuration.Time, activity.StartDate))
						{
							foreach (var notificationTypeName in configuration.NotificationTypes)
							{
								var notificationType = _notificationTypeProvider[notificationTypeName];
								notifiableService.Notify(activity.Id, notificationType);
							}
							_reminderService.SetAsDelivered(reminder.Id);
						}
					}
				}
			}
			catch (Exception ex)
			{
				_exceptionLogger.Log(ex);
			}
			finally
			{
				_isRunning = false;
			}
		}

		private ReminderTypeConfiguration GetConfiguration(ReminderTypeEnum type)
		{
			return _configurationProvider.GetSettings().Configurations.Single(c => c.Type == type);
		}

		private static bool ShouldNotify(int time, DateTime startDate)
		{
			return startDate.Subtract(DateTime.UtcNow).TotalMinutes < time;
		}
	}
}
