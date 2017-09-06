namespace uIntra.CentralFeed
{
    public class CentralFeedListModel
    { 
        public int Type { get; set; }
        public long? Version { get; set; }
        public int Page { get; set; } = 1;

        public FeedFilterStateModel FilterState { get; set; }
    }

    /// TODO : Move into separate file
    public class FeedFilterStateModel
    {
        public bool? ShowSubscribed { get; set; }
        public bool? ShowPinned { get; set; }
        public bool? IncludeBulletin { get; set; }
    }
}
