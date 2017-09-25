using System;
using System.Linq;
using System.Web;
using Compent.uIntra.Core.Constants;
using EmailWorker.Data.Domain;
using EmailWorker.Data.Model;
using EmailWorker.Data.Services.Interfaces;
using EmailWorker.Web.Helper;
using uIntra.Notification;
using uIntra.Notification.Base;
using uIntra.Core;
using uIntra.Core.Extentions;
using uIntra.Notification.Configuration;
using Umbraco.Web;

namespace Compent.uIntra.Core.Notification
{
    public class MailService : IMailService
    {
        private readonly IEmailService _emailService;
        private readonly IEmailJobSenderService _emailJobSenderService;
        private readonly ISentMailsService _sentMailsService;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IDocumentTypeAliasProvider documentTypeAliasProvider;       

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
            var email = mail as IEmailBase;
            if (email == null)
            {
                throw new NotImplementedException();
            }
            
            _emailService.AddInMailQueue(email);
        }

        public void SendOneTimePerDayMailForSpecialTypeAndDay(MailBase mail, DateTime day, NotificationTypeEnum mailTemplateTypeEnum)
        {
            int totalCount;
            var query = new EmailLogQuery
            {
                StartCreateDate = new DateTime(day.Year, day.Month, day.Day),
                TypeId = GetEmailTemplatePublishedContentId(mailTemplateTypeEnum)
            };

            _sentMailsService.GetAllByFilter(query, out totalCount);
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

        public void ProcessMails(int? count = null, int? mailId = null)
        {
            _emailJobSenderService.SendMails(string.Empty, count, mailId);
        }
    }
}