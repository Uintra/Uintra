using System;
using System.Linq;
using Uintra20.Core.Activity;
using Uintra20.Core.Configuration;
using Uintra20.Features.Notification.Configuration;
using Uintra20.Features.Notification.Configuration.BackofficeSettings.Providers;
using Uintra20.Features.Notification.Entities.Base;
using Uintra20.Features.Notification.Services;
using Uintra20.Features.Reminder.Configuration;
using Umbraco.Core.Logging;

namespace Uintra20.Features.Reminder.Services
{
    public class ReminderRunner : IReminderRunner
    {
        private readonly IReminderService _reminderService;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly IConfigurationProvider<ReminderConfiguration> _configurationProvider;
        private readonly INotificationTypeProvider _notificationTypeProvider;
        private readonly ILogger _logger;
        private static bool _isRunning;

        public ReminderRunner(
            IReminderService reminderService,
            IActivitiesServiceFactory activitiesServiceFactory,
            IConfigurationProvider<ReminderConfiguration> configurationProvider,
            INotificationTypeProvider notificationTypeProvider,
            ILogger logger)
        {
            _reminderService = reminderService;
            _activitiesServiceFactory = activitiesServiceFactory;
            _configurationProvider = configurationProvider;
            _notificationTypeProvider = notificationTypeProvider;
            _logger = logger;
        }

        public void Run()
        {
            var correlationId = Guid.NewGuid();
            if (_isRunning)
            {
                _logger.Info<ReminderRunner>("Previous reminder is still running...");
                return;
            }

            _logger.Info<ReminderRunner>($"Reminder {correlationId} starts running...");
            _isRunning = true;

            try
            {
                var reminders = _reminderService.GetAllNotDelivered();
                _logger.Info<ReminderRunner>($"{reminders.Count()} activities to remind");
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
                            _logger.Info<ReminderRunner>($"{activity.Id} activity needs to remind");
                            foreach (var notificationTypeName in configuration.NotificationTypes)
                            {
                                var notificationType = _notificationTypeProvider[notificationTypeName];
                                notifiableService.Notify(activity.Id, notificationType);
                            }

                            _reminderService.SetAsDelivered(reminder.Id);
                            _logger.Info<ReminderRunner>($"{activity.Id} set as delivered");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error<ReminderRunner>(ex.Message);
                _logger.Error<ReminderRunner>(ex.StackTrace);
            }
            finally
            {
                _isRunning = false;
                _logger.Info<ReminderRunner>($"Reminder {correlationId} finished");
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