namespace Uintra20.Features.Notification.Entities.Base.Mails
{
    public class MailAttachmentFile
    {
        public string Name { get; set; }
        public byte[] ByteContent { get; set; }
        public string PathToContent { get; set; }
    }
}