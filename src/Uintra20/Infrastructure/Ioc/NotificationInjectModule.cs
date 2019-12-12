using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Features.Notification;
using Uintra20.Features.Notification.Configuration;
using Uintra20.Features.Notification.Configuration.BackofficeSettings.Helpers;
using Uintra20.Features.Notification.Configuration.BackofficeSettings.Providers;
using Uintra20.Features.Notification.Models;
using Uintra20.Features.Notification.Models.NotifierTemplates;
using Uintra20.Features.Notification.Services;
using Uintra20.Infrastructure.Helpers;

namespace Uintra20.Infrastructure.Ioc
{
    public class NotificationInjectModule : IInjectModule
    {
        public IDependencyCollection Register(IDependencyCollection services)
        {
            services.AddScoped<INotifierService, UiNotifierService>();
            services.AddScoped<INotifierService, PopupNotifierService>();
            services.AddScoped<INotifierService, MailNotifierService>();
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

            services.AddScoped<INotificationSettingCategoryProvider, NotificationSettingCategoryProvider>();
            services.AddScoped<INotificationSettingsTreeProvider, NotificationSettingsTreeProvider>();
            //services.AddScoped<INotificationTypeProvider, NotificationTypeProvider>();
            services.AddScoped(x =>
            {
                var result = new NotificationTypeProvider(typeof(NotificationTypeEnum));
                return (INotificationTypeProvider) result;
            });

            return services;
        }
    }
}