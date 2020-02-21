using System;
using System.Linq;
using System.Threading.Tasks;
using EmailWorker.Data.Features.EmailWorker;
using UBaseline.Core.Node;
using Uintra20.Features.Notification.Configuration;
using Uintra20.Features.Notification.Entities.Base.Mails;
using Uintra20.Features.Notification.Models;

namespace Uintra20.Features.Notification.Services
{
    public class MailService : IMailService
    {
        private readonly IEmailJobSenderService _emailJobSenderService;
        private readonly ISentMailsService _sentMailsService;
        private readonly INodeModelService _nodeModelService;

        public MailService(
			IEmailJobSenderService emailJobSenderService,
			ISentMailsService sentMailsService,
            INodeModelService nodeModelService)
		{
			_emailJobSenderService = emailJobSenderService;
			_sentMailsService = sentMailsService;
            _nodeModelService = nodeModelService;
        }

        public void Send(MailBase mail)
        {
	        var email = mail as IEmailBase;
	        if (email != null)
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

        public void SendMailByTypeAndDay(MailBase mail, string email, DateTime date,
            NotificationTypeEnum mailTemplateTypeEnum)
        {
			var query = new EmailLogQuery
			{
				StartCreateDate = new DateTime(date.Year, date.Month, date.Day),
				TypeId = GetEmailTemplateId(mailTemplateTypeEnum),
				ToEmail = email
			};

			_sentMailsService.GetAllByFilter(query, out var totalCount);
			if (totalCount == 0)
			{
				Send(mail);
			}
		}

        public async Task SendAsync(MailBase mail)
        {
	        var email = mail as IEmailBase;
	        if (email != null)
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
					await _sentMailsService.InsertAsync(sentMailsModel);
				}
			}
			else
			{
				throw new NotImplementedException();
			}
        }

        public async Task SendMailByTypeAndDayAsync(MailBase mail, string email, DateTime date, NotificationTypeEnum mailTemplateTypeEnum)
        {
			var query = new EmailLogQuery
			{
				StartCreateDate = new DateTime(date.Year, date.Month, date.Day),
				TypeId = GetEmailTemplateId(mailTemplateTypeEnum),
				ToEmail = email
			};

			_sentMailsService.GetAllByFilter(query, out var totalCount);
			if (totalCount == 0)
			{
				await SendAsync(mail);
			}
		}

        private int? GetEmailTemplateId(NotificationTypeEnum mailTemplateTypeEnum)
        {
            var mailTemplates = _nodeModelService.AsEnumerable().OfType<MailTemplateModel>();

            var mailTemplateContent = mailTemplates.FirstOrDefault(template => template.EmailType.Value == mailTemplateTypeEnum.ToString());
            return mailTemplateContent?.Id;
        }
    }
}
