using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Compent.Uintra.Core.Updater;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Steps.AggregateSubsteps;
using Extensions;
using uIntra.Notification;
using Uintra.Core;
using Uintra.Core.Exceptions;
using Uintra.Core.Extensions;
using Uintra.Notification;
using Uintra.Notification.Configuration;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;


namespace Compent.Uintra.Core.Updater.Migrations._0._2._32._0.Steps
{
    public class NotificationSettingsMigrationStep : IMigrationStep
    {
        private readonly IEnumerable<IPublishedContent> _mailTemplates;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly INotificationSettingsService _notificationSettingsService;
        private readonly IExceptionLogger _exceptionLogger;
        private readonly IContentService _contentService;

        public NotificationSettingsMigrationStep(
            UmbracoHelper umbracoHelper,
            IDocumentTypeAliasProvider documentTypeAliasProvider,
            INotificationSettingsService notificationSettingsService,
            IExceptionLogger exceptionLogger,
            IContentService contentService)
        {
            _umbracoHelper = umbracoHelper;
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _notificationSettingsService = notificationSettingsService;
            _exceptionLogger = exceptionLogger;
            _contentService = contentService;
            _mailTemplates = GetMailTemplates();
        }

        public ExecutionResult Execute()
        {
            ImportSettingsFromMailTemplates(CommunicationTypeEnum.CommunicationSettings, NotificationTypeEnum.CommentLikeAdded);
            ImportSettingsFromMailTemplates(CommunicationTypeEnum.CommunicationSettings, NotificationTypeEnum.MonthlyMail);
            DeleteExistedMailTemplates();


            return ExecutionResult.Success;
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        private void DeleteExistedMailTemplates()
        {

            var mailTemplateFolderXpath = XPathHelper.GetXpath(
                _documentTypeAliasProvider.GetDataFolder(),
                _documentTypeAliasProvider.GetMailTemplateFolder());

            var publishedContent = _umbracoHelper.TypedContentSingleAtXPath(mailTemplateFolderXpath);

            bool IsForRemove(IPublishedContent content)
            {
                var templateType = content.GetPropertyValue<NotificationTypeEnum>(UmbracoContentMigrationConstants.MailTemplate.EmailTypePropName);

                return templateType.In(NotificationTypeEnum.CommentLikeAdded, NotificationTypeEnum.MonthlyMail);
            }

            var publishedContentToRemove = publishedContent.Children.Where(IsForRemove);
            var contentToRemove = _contentService.GetByIds(publishedContentToRemove.Select(c => c.Id)).ToList();
            contentToRemove.ForEach(c => _contentService.Delete(c));
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