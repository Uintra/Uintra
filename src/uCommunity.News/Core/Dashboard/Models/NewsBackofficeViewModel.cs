using System;

namespace uCommunity.News.Dashboard
{
    public class NewsBackofficeViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Teaser { get; set; }
        public string Media { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime PublishDate { get; set; }
        public bool IsHidden { get; set; }
        public DateTime ModifyDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}