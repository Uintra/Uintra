using System;
using EmailWorker.Data.Model;
using EmailWorker.Data.Services.Interfaces;
using EmailWorker.Web.Helper;
using uIntra.Notification;
using uIntra.Notification.Base;

namespace uIntra.Core.Notification
{
    public class MailService : IMailService
    {
        private readonly IEmailService _emailService;
        private readonly IEmailJobSenderService _emailJobSenderService;

        public MailService(
            IEmailService emailService, 
            IEmailJobSenderService emailJobSenderService)
        {
            _emailService = emailService;
            _emailJobSenderService = emailJobSenderService;
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

        public void ProcessMails(int? count = null, int? mailId = null)
        {
            _emailJobSenderService.SendMails(string.Empty, count, mailId);
        }
    }
}