namespace uIntra.Notification
{
    public class EmailNotifierTemplate : INotifierTemplate
    {
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}