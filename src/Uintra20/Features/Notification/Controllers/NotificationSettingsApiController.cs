using Compent.Extensions;
using System.Web.Http;
using Uintra20.Features.Notification.Configuration;
using Uintra20.Features.Notification.Configuration.BackofficeSettings.Providers;
using Uintra20.Features.Notification.Models;
using Uintra20.Features.Notification.Models.Configuration;
using Uintra20.Features.Notification.Models.NotifierTemplates;
using Uintra20.Features.Notification.Services;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.TypeProviders;
using Umbraco.Web.WebApi;

namespace Uintra20.Features.Notification.Controllers
{
    public class NotificationSettingsApiController : UmbracoAuthorizedApiController
    {
        private readonly INotificationSettingsService _notificationSettingsService;
        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly INotificationTypeProvider _notificationTypeProvider;
        private readonly INotifierTypeProvider _notifierTypeProvider;


        public NotificationSettingsApiController(
            INotificationSettingsService notificationSettingsService,
            IActivityTypeProvider activityTypeProvider,
            INotificationTypeProvider notificationTypeProvider,
            INotifierTypeProvider notifierTypeProvider)
        {
            _notificationSettingsService = notificationSettingsService;
            _activityTypeProvider = activityTypeProvider;
            _notificationTypeProvider = notificationTypeProvider;
            _notifierTypeProvider = notifierTypeProvider;
        }

        [HttpGet]
        public virtual NotifierSettingsModel Get(int activityType, int notificationType)
        {
            ActivityEventIdentity activityEventIdentity;
            NotifierSettingsModel settings;

            if (activityType == CommunicationTypeEnum.CommunicationSettings.ToInt())
            {
                activityEventIdentity = new ActivityEventIdentity(CommunicationTypeEnum.CommunicationSettings,
                    _notificationTypeProvider[notificationType]);
                settings = _notificationSettingsService.GetSettings(activityEventIdentity);
                if (notificationType.In(NotificationTypeEnum.MonthlyMail.ToInt()))
                {
                    settings.UiNotifierSetting = null;
                    settings.PopupNotifierSetting = null;
                }

                return settings;
            }

            if (activityType == CommunicationTypeEnum.Member.ToInt())
            {
                activityEventIdentity = new ActivityEventIdentity(CommunicationTypeEnum.Member,
                    _notificationTypeProvider[notificationType]);
                settings = _notificationSettingsService.GetSettings(activityEventIdentity);
                if (notificationType.In(NotificationTypeEnum.Welcome.ToInt()))
                {
                    settings.UiNotifierSetting = null;
                    settings.EmailNotifierSetting = null;
                }

                return settings;
            }
            var actType = _activityTypeProvider[activityType];
            activityEventIdentity = new ActivityEventIdentity(actType, _notificationTypeProvider[notificationType]);
            settings = _notificationSettingsService.GetSettings(activityEventIdentity);
            settings.PopupNotifierSetting = null;

            return settings;
        }

        [HttpPost]
        public virtual void SaveUiNotifierSetting(NotifierSettingSaveModel<UiNotifierTemplate> notifierSettingModel)
        {
            var mappedModel = notifierSettingModel.Map<NotifierSettingModel<UiNotifierTemplate>>();
            FillEnumTypes(mappedModel, notifierSettingModel);

            _notificationSettingsService.Save(mappedModel);
        }

        [HttpPost]
        public virtual void SaveEmailNotifierSetting(NotifierSettingSaveModel<EmailNotifierTemplate> notifierSettingModel)
        {
            var mappedModel = notifierSettingModel.Map<NotifierSettingModel<EmailNotifierTemplate>>();
            FillEnumTypes(mappedModel, notifierSettingModel);

            _notificationSettingsService.Save(mappedModel);
        }

        [HttpPost]
        public virtual void SavePopupNotifierSetting(NotifierSettingSaveModel<PopupNotifierTemplate> notifierSettingModel)
        {
            var mappedModel = notifierSettingModel.Map<NotifierSettingModel<PopupNotifierTemplate>>();
            FillEnumTypes(mappedModel, notifierSettingModel);

            _notificationSettingsService.Save(mappedModel);
        }


        [HttpPost]
        public virtual void SaveDesktopNotifierSetting(NotifierSettingSaveModel<DesktopNotifierTemplate> notifierSettingModel)
        {
            var mappedModel = notifierSettingModel.Map<NotifierSettingModel<DesktopNotifierTemplate>>();
            FillEnumTypes(mappedModel, notifierSettingModel);

            _notificationSettingsService.Save(mappedModel);
        }

        protected virtual void FillEnumTypes<T>(
            NotifierSettingModel<T> notifierSettingModel,
            NotifierSettingSaveModel<T> notifierSettingSaveModel)
            where T : INotifierTemplate
        {

            if (notifierSettingSaveModel.ActivityType == CommunicationTypeEnum.CommunicationSettings.ToInt())
                notifierSettingModel.ActivityType = CommunicationTypeEnum.CommunicationSettings;
            else if (notifierSettingSaveModel.ActivityType == CommunicationTypeEnum.Member.ToInt())
                notifierSettingModel.ActivityType = CommunicationTypeEnum.Member;
            else
                notifierSettingModel.ActivityType = _activityTypeProvider[notifierSettingSaveModel.ActivityType];

            notifierSettingModel.NotificationType = _notificationTypeProvider[notifierSettingSaveModel.NotificationType];
            notifierSettingModel.NotifierType = _notifierTypeProvider[notifierSettingSaveModel.NotifierType];
        }
    }
}