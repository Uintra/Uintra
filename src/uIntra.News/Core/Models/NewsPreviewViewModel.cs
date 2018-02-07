using System;
using uIntra.Core.Links;
using uIntra.Core.User;

namespace uIntra.News
{
    public class NewsPreviewViewModel
    {
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public IIntranetUser Owner { get; set; }
        public Enum ActivityType { get; set; }
        public Guid Id { get; set; }
        public ActivityLinks Links { get; set; }
    }
}