using System;

namespace uCommunity.News.Dashboard
{
    public class NewsBackofficeSaveModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Teaser { get; set; }
        public string Media { get; set; }
        public int UmbracoCreatorId { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime? UnpublishDate { get; set; }
        public bool IsHidden { get; set; }
    }
}