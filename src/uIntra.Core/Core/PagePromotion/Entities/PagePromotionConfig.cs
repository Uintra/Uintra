using System;

namespace uIntra.Core.PagePromotion
{
    public class PagePromotionConfig
    {
        public bool PromoteOnCentralFeed { get; set; }
        public bool Likeable { get; set; }
        public bool Commentable { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public string Files { get; set; }
    }
}
