using System;
using Uintra.Core.Links;
using Uintra.Core.User;

namespace Uintra.News
{
    public class NewsPreviewViewModel
    {
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public UserViewModel Owner { get; set; }
        public Enum ActivityType { get; set; }
        public Guid Id { get; set; }
        public ActivityLinks Links { get; set; }
    }
}