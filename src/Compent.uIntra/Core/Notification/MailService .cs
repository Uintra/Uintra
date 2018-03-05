using System;
using System.Linq;
using System.Web;
using Compent.Uintra.Core.Constants;
using EmailWorker.Data.Domain;
using EmailWorker.Data.Model;
using EmailWorker.Data.Services.Interfaces;
using EmailWorker.Web.Helper;
using Uintra.Core;
using Uintra.Core.Extensions;
using Uintra.Notification;
using Uintra.Notification.Base;
using Uintra.Notification.Configuration;
using Umbraco.Web;

namespace Compent.Uintra.Core.Notification
{
    public class MailService : IMailService
    {
        private readonly IEmailService _emailService;
        private readonly IEmailJobSenderService _emailJobSenderService;
        private readonly ISentMailsService _sentMailsService;
        private readonly UmbracoHelper _umbracoHelper;

        public MailService(
            IEmailService emailService,
            IEmailJobSenderService emailJobSenderService,
            ISentMailsService sentMailsService,
            UmbracoHelper umbracoHelper)
        {
            _emailService = emailService;
            _emailJobSenderService = emailJobSenderService;
            _sentMailsService = sentMailsService;
            _umbracoHelper = umbracoHelper;
        }

        public void Send(MailBase mail)
        {
            if (mail is IEmailBase email)
            {
                foreach (var recipient in mail.Recipients)
                {
                    var sentMailsModel = new SentMails
                    {
                        Body = email.Body,
                        Subject = email.Subject,
                        IsSent = false,
                        CreatedUtcDate = DateTime.UtcNow,
                        FromEmail = email.FromEmail ?? string.Empty,
                        FromName = email.FromName ?? string.Empty,
                        ToEmail = recipient.Email,
                        ToName = recipient.Name,
                        
                    };
                    _sentMailsService.Insert(sentMailsModel);
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void ProcessMails(int? count = null, int? mailId = null)
        {
            _emailJobSenderService.SendMails(string.Empty, count, mailId);
        }

        public void SendMailByTypeAndDay(MailBase mail, string email, DateTime date, NotificationTypeEnum mailTemplateTypeEnum)
        {
            var query = new EmailLogQuery
            {
                StartCreateDate = new DateTime(date.Year, date.Month, date.Day),
                TypeId = GetEmailTemplatePublishedContentId(mailTemplateTypeEnum),
                ToEmail = email
            };

            _sentMailsService.GetAllByFilter(query, out var totalCount);
            if (totalCount == 0)
            {
                Send(mail);
            }
        }

        private int? GetEmailTemplatePublishedContentId(NotificationTypeEnum mailTemplateTypeEnum)
        {
            var docTypeAliasProvider = HttpContext.Current.GetService<IDocumentTypeAliasProvider>();
            string mailTemplateXpath = XPathHelper.GetXpath(docTypeAliasProvider.GetDataFolder(), docTypeAliasProvider.GetMailTemplateFolder(), docTypeAliasProvider.GetMailTemplate());
            var mailTemplates = _umbracoHelper.TypedContentAtXPath(mailTemplateXpath);
            var mailTemplateContent = mailTemplates?.FirstOrDefault(template => template.GetPropertyValue<NotificationTypeEnum>(MailTemplatePropertiesConstants.EmailType) == mailTemplateTypeEnum);
            return mailTemplateContent?.Id;
        }
    }
}