namespace uIntra.Notification
{
    public class EmailNotifierTemplate : INotifierTemplate
    {
        public string Body { get; set; }
        public string Subject { get; set; }
    }
}