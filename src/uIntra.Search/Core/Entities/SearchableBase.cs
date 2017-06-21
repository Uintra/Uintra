namespace uIntra.Search.Core.Entities
{
    public class SearchableBase
    {
        public object Id { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public SearchableType Type { get; set; }
    }
}