using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Extensions;
using Uintra.Core;
using Uintra.Core.Activity;
using Uintra.Core.Exceptions;
using Uintra.Core.Extensions;
using Uintra.Core.Localization;
using Uintra.Core.TypeProviders;
using Uintra.Notification;
using Uintra.Notification.Configuration;
using Uintra.Notification.Constants;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Steps.AggregateSubsteps
{
    public class NotificationSettingsMigrations 
    {
        private readonly NotificationTypeEnum[] eventNotificationTypes =
            {
                NotificationTypeEnum.EventUpdated,
                NotificationTypeEnum.EventHided,
                NotificationTypeEnum.BeforeStart,
                NotificationTypeEnum.CommentAdded,
                NotificationTypeEnum.CommentEdited,
                NotificationTypeEnum.CommentReplied,
                NotificationTypeEnum.ActivityLikeAdded
            };

        private readonly NotificationTypeEnum[] newsNotificationTypes =
            {
                NotificationTypeEnum.CommentAdded, NotificationTypeEnum.CommentEdited, NotificationTypeEnum.CommentReplied, NotificationTypeEnum.ActivityLikeAdded
            };

        private readonly NotificationTypeEnum[] bulletinNotificationTypes =
            {
                NotificationTypeEnum.CommentAdded, NotificationTypeEnum.CommentEdited, NotificationTypeEnum.CommentReplied, NotificationTypeEnum.ActivityLikeAdded
            };

        private readonly IEnumerable<IPublishedContent> mailTemplates;

        private readonly UmbracoHelper _umbracoHelper = DependencyResolver.Current.GetService<UmbracoHelper>();
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider = DependencyResolver.Current.GetService<IDocumentTypeAliasProvider>();
        private readonly INotificationSettingsService _notificationSettingsService = DependencyResolver.Current.GetService<INotificationSettingsService>();
        private readonly IActivityTypeProvider _activityTypeProvider = DependencyResolver.Current.GetService<IActivityTypeProvider>();
        private readonly INotificationTypeProvider _notificationTypeProvider = DependencyResolver.Current.GetService<INotificationTypeProvider>();
        private readonly INotifierTypeProvider _notifierTypeProvider = DependencyResolver.Current.GetService<INotifierTypeProvider>();
        private readonly IExceptionLogger _exceptionLogger = DependencyResolver.Current.GetService<IExceptionLogger>();
        private readonly IIntranetLocalizationService _localizationService = DependencyResolver.Current.GetService<IIntranetLocalizationService>();

        public NotificationSettingsMigrations()
        {
            mailTemplates = GetMailTemplates();
        }

        public void Execute()
        {
            ImportExistedEmailNotificationSettings();
            ImportExistedUiNotificationSettings();
        }

        private void ImportExistedEmailNotificationSettings()
        {
            foreach (var eventNotificationType in eventNotificationTypes)
            {
                ImportSettingsFromMailTemplates(IntranetActivityTypeEnum.Events, eventNotificationType);
            }

            foreach (var newsNotificationType in newsNotificationTypes)
            {
                ImportSettingsFromMailTemplates(IntranetActivityTypeEnum.News, newsNotificationType);
            }

            foreach (var bulletinNotificationType in bulletinNotificationTypes)
            {
                ImportSettingsFromMailTemplates(IntranetActivityTypeEnum.Bulletins, bulletinNotificationType);
            }
        }

        private IEnumerable<IPublishedContent> GetMailTemplates()
        {
            var mailTemplateXpath = XPathHelper.GetXpath(_documentTypeAliasProvider.GetDataFolder(), _documentTypeAliasProvider.GetMailTemplateFolder(), _documentTypeAliasProvider.GetMailTemplate());
            return _umbracoHelper.TypedContentAtXPath(mailTemplateXpath);
        }

        private NotifierSettingModel<EmailNotifierTemplate> GetEmailNotifierSettings(IntranetActivityTypeEnum activityType, NotificationTypeEnum notificationType)
        {
            return _notificationSettingsService.Get<EmailNotifierTemplate>(GetActivityEventNotifierIdentity(NotifierTypeEnum.EmailNotifier, activityType, notificationType));
        }

        private void ImportSettingsFromMailTemplates(IntranetActivityTypeEnum activityType, NotificationTypeEnum notificationType)
        {
            var notifierSettings = GetEmailNotifierSettings(activityType, notificationType);
            if (notifierSettings == null)
            {
                _exceptionLogger.Log(new NullReferenceException($"Email notifier settings for activity type = {activityType} and notification type = {notificationType} doesn't exist."));
                return;
            }

            var mailTemplate = mailTemplates.SingleOrDefault(template => template.GetPropertyValue<NotificationTypeEnum>("emailType") == notificationType);
            if (mailTemplate == null) return;

            var mailSubject = mailTemplate.GetPropertyValue<string>("subject");
            if (mailSubject.HasValue())
            {
                notifierSettings.Template.Subject = mailSubject;
            }

            var mailBody = mailTemplate.GetPropertyValue<string>("body");
            if (mailBody.HasValue())
            {
                notifierSettings.Template.Body = mailBody;
            }

            _notificationSettingsService.Save(notifierSettings);
        }

        private void ImportExistedUiNotificationSettings()
        {
            foreach (var eventNotificationType in eventNotificationTypes)
            {
                ImportUiSettings(IntranetActivityTypeEnum.Events, eventNotificationType);
            }

            foreach (var newsNotificationType in newsNotificationTypes)
            {
                ImportUiSettings(IntranetActivityTypeEnum.News, newsNotificationType);
            }

            foreach (var bulletinNotificationType in bulletinNotificationTypes)
            {
                ImportUiSettings(IntranetActivityTypeEnum.Bulletins, bulletinNotificationType);
            }
        }

        private void ImportUiSettings(IntranetActivityTypeEnum activityType, NotificationTypeEnum notificationType)
        {
            var notifierSettings = GetUiNotifierSettings(activityType, notificationType);
            if (notifierSettings == null)
            {
                _exceptionLogger.Log(new NullReferenceException($"Ui notifier settings for activity type = {activityType} and notification type = {notificationType} doesn't exist."));
                return;
            }

            var uiNotificationMessage = GetUiNotificationMessage(notificationType);
            if (uiNotificationMessage.HasValue())
            {
                notifierSettings.Template.Message = uiNotificationMessage;
            }

            _notificationSettingsService.Save(notifierSettings);
        }

        private NotifierSettingModel<UiNotifierTemplate> GetUiNotifierSettings(IntranetActivityTypeEnum activityType, NotificationTypeEnum notificationType)
        {
            return _notificationSettingsService.Get<UiNotifierTemplate>(GetActivityEventNotifierIdentity(NotifierTypeEnum.UiNotifier, activityType, notificationType));
        }

        private string GetUiNotificationMessage(NotificationTypeEnum notificationType)
        {
            var message = string.Empty;

            switch (notificationType)
            {
                case NotificationTypeEnum.ActivityLikeAdded:
                case NotificationTypeEnum.CommentAdded:
                case NotificationTypeEnum.CommentEdited:
                case NotificationTypeEnum.CommentReplied:
                case NotificationTypeEnum.EventUpdated:
                case NotificationTypeEnum.EventHided:
                    message = $"<strong>{TokensConstants.FullName} {_localizationService.Translate(notificationType.ToString())}</strong>"
                              + $"<p>{TokensConstants.ActivityTitle}</p>";
                    break;

                case NotificationTypeEnum.BeforeStart:
                    message = $"<strong>{_localizationService.Translate(notificationType.ToString())}</strong>"
                              + $"<p>{TokensConstants.ActivityTitle}</p>";
                    break;
            }

            return message;
        }

        private ActivityEventNotifierIdentity GetActivityEventNotifierIdentity(NotifierTypeEnum notifierType, IntranetActivityTypeEnum activityType, Enum notificationType)
        {
            var activityEventIdentity = new ActivityEventIdentity(activityType, notificationType);
            return new ActivityEventNotifierIdentity(activityEventIdentity, notifierType);
        }
    }
}
