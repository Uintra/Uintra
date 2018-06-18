using System;
using System.Collections.Generic;
using EmailWorker.Data.Model;
using uIntra.Core.Extensions;
using uIntra.Notification.MailModels;

namespace Compent.uIntra.Core.Notification.Mails
{
    public class MonthlyMail : MonthlyMailBase, IEmailBase
    {
        public string GetXPath()
        {
            return MailConfiguration.MailTemplateXpath;
        }

        public IList<IEmailRecipient> To => Recipients.Map<IList<IEmailRecipient>>();
        public IEnumerable<IEmailAttachmentFile> AttachmentFiles => Attachments.Map<IList<IEmailAttachmentFile>>();
        public Enum MailTemplateTypeEnum => MailTemplateType;
    }
}