using Uintra.Core.Member.Models;

namespace Uintra.Core.Feed.Models
{
    public class FeedActivityViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public MemberViewModel Creator { get; set; }
        public string DateString { get; set; }
    }
}