namespace Uintra20.Core.Notification
{
    public class EmailNotifierTemplate : INotifierTemplate
    {
        public string Body { get; set; }
        public string Subject { get; set; }
    }
}