namespace Uintra20.Features.Search.Entities.Mappings
{
    public class SearchableActivityMap : SearchableBaseMap<SearchableActivity>
    {
        public SearchableActivityMap()
        {
            Text(t => t.Name(n => n.Description).Analyzer(ElasticHelpers.ReplaceNgram));
        }
    }
}