namespace Uintra20.Core.Feed.Models
{
    public class FeedListModel
    {
        public int TypeId { get; set; }
        public int Page { get; set; } = 1;
        public FeedFilterStateModel FilterState { get; set; }
    }
}