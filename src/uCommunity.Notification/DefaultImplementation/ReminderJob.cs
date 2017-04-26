using System;
using System.Linq;
using uCommunity.Core.Activity;
using uCommunity.Core.Configuration;
using uCommunity.Core.Exceptions;
using uCommunity.Notification.Core.Configuration;
using uCommunity.Notification.Core.Entities.Base;
using uCommunity.Notification.Core.Services;

namespace uCommunity.Notification
{
    public class ReminderJob : IReminderJob
    {
        private readonly IReminderService _reminderService;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly IConfigurationProvider<ReminderConfiguration> _configurationProvider;
        private readonly IExceptionLogger _exceptionLogger;
        private static bool _isRunning;

        public ReminderJob(
            IReminderService reminderService,
            IActivitiesServiceFactory activitiesServiceFactory,
            IConfigurationProvider<ReminderConfiguration> configurationProvider,
            IExceptionLogger exceptionLogger)
        {
            _reminderService = reminderService;
            _activitiesServiceFactory = activitiesServiceFactory;
            _configurationProvider = configurationProvider;
            _exceptionLogger = exceptionLogger;
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
                    var reminderService = _activitiesServiceFactory.GetService<IReminderableService<IReminderable>>(reminder.ActivityId);
                    var notifyableService = _activitiesServiceFactory.GetService<INotifyableService>(reminder.ActivityId);

                    if (reminderService == null || notifyableService == null)
                    {
                        continue;
                    }

                    var activity = reminderService.Get(reminder.ActivityId);
                    var configuration = GetConfiguration(reminder.Type);
                    if (ShouldNotify(configuration.Time, activity.StartDate))
                    {
                        foreach (var notificationType in configuration.NotificationTypes)
                        {
                            notifyableService.Notify(activity.Id, notificationType);
                        }
                        _reminderService.SetAsDelivered(reminder.Id);
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
            return startDate.Subtract(DateTime.Now).TotalMinutes < time;
        }
    }
}
