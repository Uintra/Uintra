namespace uIntra.Search
{
    public static class SearchConstants
    {
        public const int FragmentSize = 100;// depends on elastic fragment_size setting for highlighting (by default 100)

        public const string HighlightPreTag = "<em style='background:#ffffc0'>";
        public const string HighlightPostTag = "</em>";
    }
}
