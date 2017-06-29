namespace uIntra.Search
{
    public class SearchTextQuery
    {
        public string Text { get; set; }

        public int Take { get; set; }

        public bool ApplyHighlights { get; set; }
    }
}