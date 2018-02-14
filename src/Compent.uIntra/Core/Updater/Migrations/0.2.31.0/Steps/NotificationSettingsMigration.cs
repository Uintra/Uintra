using System;
using System.Collections.Generic;
using System.Linq;
using Compent.Uintra.Core.Updater;
using Extensions;
using uIntra.Notification;
using Uintra.Core;
using Uintra.Core.Exceptions;
using Uintra.Core.Extensions;
using Uintra.Notification;
using Uintra.Notification.Configuration;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Compent.uIntra.Core.Updater.Migrations._0._2._31._0.Steps
{
    public class NotificationSettingsMigration : IMigrationStep
    {
        private readonly IEnumerable<IPublishedContent> _mailTemplates;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly INotificationSettingsService _notificationSettingsService;
        private readonly IExceptionLogger _exceptionLogger;

        public NotificationSettingsMigration(
            UmbracoHelper umbracoHelper,
            IDocumentTypeAliasProvider documentTypeAliasProvider,
            INotificationSettingsService notificationSettingsService,
            IExceptionLogger exceptionLogger)
        {
            _umbracoHelper = umbracoHelper;
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _notificationSettingsService = notificationSettingsService;
            _exceptionLogger = exceptionLogger;
            _mailTemplates = GetMailTemplates();
        }

        public ExecutionResult Execute()
        {
            ImportSettingsFromMailTemplates(CommunicationTypeEnum.CommunicationSettings, NotificationTypeEnum.CommentLikeAdded);
            ImportSettingsFromMailTemplates(CommunicationTypeEnum.CommunicationSettings, NotificationTypeEnum.MonthlyMail);
            return ExecutionResult.Success;
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        private IEnumerable<IPublishedContent> GetMailTemplates()
        {
            var mailTemplateXpath = XPathHelper.GetXpath(_documentTypeAliasProvider.GetDataFolder(), _documentTypeAliasProvider.GetMailTemplateFolder(),
                _documentTypeAliasProvider.GetMailTemplate());
            return _umbracoHelper.TypedContentAtXPath(mailTemplateXpath);
        }

        private NotifierSettingModel<EmailNotifierTemplate> GetEmailNotifierSettings(Enum activityType, Enum notificationType)
        {
            return _notificationSettingsService.Get<EmailNotifierTemplate>(GetActivityEventNotifierIdentity(NotifierTypeEnum.EmailNotifier, activityType,
                notificationType));
        }

        private void ImportSettingsFromMailTemplates(Enum activityType, Enum notificationType)
        {
            var notifierSettings = GetEmailNotifierSettings(activityType, notificationType);
            if (notifierSettings == null)
            {
                _exceptionLogger.Log(
                    new NullReferenceException($"Email notifier settings for activity type = {activityType} and notification type = {notificationType} doesn't exist."));
                return;
            }

            var mailTemplate = _mailTemplates.SingleOrDefault(template =>
                template.GetPropertyValue<NotificationTypeEnum>("emailType").ToInt() == notificationType.ToInt());
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

        private ActivityEventNotifierIdentity GetActivityEventNotifierIdentity(Enum notifierType, Enum activityType, Enum notificationType)
        {
            var activityEventIdentity = new ActivityEventIdentity(activityType, notificationType);
            return new ActivityEventNotifierIdentity(activityEventIdentity, notifierType);
        }
    }
}