using Compent.Shared.DependencyInjection.Contract;
using FluentScheduler;
using Uintra.Core.Configuration;
using Uintra.Core.Jobs;
using Uintra.Core.Jobs.Configuration;
using Uintra.Core.Jobs.Models;
using Uintra.Features.Jobs;
using Uintra.Features.MonthlyMail;
using Microsoft.AspNet.SignalR;
using Uintra.Core.Hubs;
using Uintra.Features.Notification;
using Uintra.Features.Notification.Configuration;
using Uintra.Features.Notification.Configuration.BackofficeSettings.Helpers;
using Uintra.Features.Notification.Configuration.BackofficeSettings.Providers;
using Uintra.Features.Notification.Models;
using Uintra.Features.Notification.Models.NotifierTemplates;
using Uintra.Features.Notification.Services;
using Uintra.Features.Notification.Settings;
using Uintra.Features.Reminder.Configuration;
using Uintra.Features.Reminder.Services;
using Uintra.Infrastructure.Helpers;

namespace Uintra.Infrastructure.Ioc
{
    public class NotificationInjectModule : IInjectModule
    {
        public IDependencyCollection Register(IDependencyCollection services)
        {
            services.AddScopedToCollection<INotifierService, UiNotifierService>();
            services.AddScopedToCollection<INotifierService, PopupNotifierService>();
            services.AddScopedToCollection<INotifierService, MailNotifierService>();
            services.AddScoped<INotificationsService, NotificationsService>();
            services.AddScoped<IUiNotificationService, UiNotificationService>();
            services.AddScoped<IPopupNotificationService, PopupNotificationsService>();
            services.AddScoped<IMemberNotifiersSettingsService, MemberNotifiersSettingsService>();
            services.AddScoped<IMailService, MailService>();
            services.AddTransient<INotificationSettingsService, NotificationSettingsService>();
            services.AddScoped<INotificationModelMapper<UiNotifierTemplate, UiNotificationMessage>, UiNotificationModelMapper>();
            services.AddScoped<INotificationModelMapper<PopupNotifierTemplate, PopupNotificationMessage>, PopupNotificationModelMapper>();
            services.AddScoped<INotificationModelMapper<EmailNotifierTemplate, EmailNotificationMessage>, MailNotificationModelMapper>();
            services.AddScoped<INotificationModelMapper<DesktopNotifierTemplate, DesktopNotificationMessage>, DesktopNotificationModelMapper>();
            services.AddScoped<INotifierTypeProvider>(provider => new NotifierTypeProvider(typeof(NotifierTypeEnum)));
            services.AddTransient<IBackofficeNotificationSettingsProvider, BackofficeNotificationSettingsProvider>();
            services.AddTransient<IBackofficeSettingsReader, BackofficeSettingsReader>();
            services.AddScoped<INotifierDataHelper, NotifierDataHelper>();
            services.AddScoped<INotifierDataBuilder, NotifierDataBuilder>();
            services.AddConfiguration<NotificationSettings>();

            services.AddScoped<INotificationSettingCategoryProvider, NotificationSettingCategoryProvider>();
            services.AddScoped<INotificationSettingsTreeProvider, NotificationSettingsTreeProvider>();
            services.AddScoped<INotificationContentProvider, NotificationContentProvider>();
            services.AddScoped(x =>
            {
                var result = new NotificationTypeProvider(typeof(NotificationTypeEnum));
                return (INotificationTypeProvider) result;
            });

            services.AddScopedToCollection<UintraBaseIntranetJob, ReminderJob>();
            services.AddScopedToCollection<UintraBaseIntranetJob, MontlyMailJob>();
            services.AddScopedToCollection<UintraBaseIntranetJob, SendEmailJob>();
			services.AddScopedToCollection<UintraBaseIntranetJob, UpdateActivityCacheJob>();
			services.AddScopedToCollection<UintraBaseIntranetJob, GdprMailsJob>();
			services.AddScoped<IReminderRunner, ReminderRunner>();
			services.AddScoped<IReminderService, ReminderService>();
			services.AddScoped<IMonthlyEmailService,MonthlyEmailService>();
			services.AddTransient<IJobFactory, IntranetJobFactory>();

			services.AddSingleton<IConfigurationProvider<ReminderConfiguration>>(i =>
			{
				var provider= new ConfigurationProvider<ReminderConfiguration>("Uintra.Features.Reminder.Configuration.reminderConfiguration.json");
				provider.Initialize();
				return provider;
			});
			services.AddSingleton<IJobSettingsConfiguration>(i => JobSettingsConfiguration.Configure);

            services.AddScoped<UintraHub>();

            services.AddScoped<IUserIdProvider, SignalRUserIdProvider>();
            services.AddScoped<IUserMentionNotificationService, UserMentionNotificationService>();

            return services;
        }
    }
}