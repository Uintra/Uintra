namespace Uintra20.Features.Notification.Models.NotifierTemplates
{
    public class EmailNotifierTemplate : INotifierTemplate
    {
        public string Body { get; set; }
        public string Subject { get; set; }
    }
}