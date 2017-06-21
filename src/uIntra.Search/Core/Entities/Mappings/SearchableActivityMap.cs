namespace uIntra.Search.Core.Entities.Mappings
{
    public class SearchableActivityMap : SearchableBaseMap<SearchableActivity>
    {
        public SearchableActivityMap()
        {
            Text(t => t.Name(n => n.Teaser).Analyzer(ElasticHelpers.ReplaceNgram));
            Text(t => t.Name(n => n.Description).Analyzer(ElasticHelpers.ReplaceNgram));
        }
    }
}