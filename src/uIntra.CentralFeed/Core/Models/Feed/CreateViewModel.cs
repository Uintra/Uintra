using Uintra.Core.Links;

namespace Uintra.CentralFeed
{
    public class CreateViewModel
    {
        public IActivityCreateLinks Links { get; set; }
        public FeedSettings Settings { get; set; }
    }
}