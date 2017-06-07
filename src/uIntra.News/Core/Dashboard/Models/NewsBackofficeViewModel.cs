using System;

namespace uIntra.News.Dashboard
{
    public class NewsBackofficeViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Media { get; set; }
        public Guid CreatorId { get; set; }
        public string PublishDate { get; set; }
        public string UnpublishDate { get; set; }
        public bool IsHidden { get; set; }
        public string ModifyDate { get; set; }
        public string CreatedDate { get; set; }
    }
}