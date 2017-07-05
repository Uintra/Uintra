using System;
using EmailWorker.Data.Model;
using EmailWorker.Data.Services.Interfaces;
using uIntra.Notification;
using uIntra.Notification.Base;

namespace Compent.uIntra.Core.Notification
{
    public class MailService : IMailService
    {
        private readonly IEmailService _emailService;

        public MailService(IEmailService emailService)
        {
            _emailService = emailService;
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
    }
}