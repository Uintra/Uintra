namespace uIntra.Search.Core
{
    public class SearchableActivityMap : SearchableBaseMap<SearchableActivity>
    {
        public SearchableActivityMap()
        {
            Text(t => t.Name(n => n.Description).Analyzer(ElasticHelpers.ReplaceNgram));
        }
    }
}