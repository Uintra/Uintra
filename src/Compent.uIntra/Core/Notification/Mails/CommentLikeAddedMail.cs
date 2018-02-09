using System;
using System.Collections.Generic;
using EmailWorker.Data.Model;
using Uintra.Core.Extensions;
using Uintra.Notification.MailModels;

namespace Compent.Uintra.Core.Notification.Mails
{
    public class CommentLikeAddedMail : CommentLikeAddedMailBase, IEmailBase
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