namespace Uintra20.Features.Search.Entities.Mappings
{
    public class SearchableUintraActivityMap : SearchableBaseMap<SearchableUintraActivity>
    {
        public SearchableUintraActivityMap()
        {
            Text(t => t.Name(n => n.Description).Analyzer(ElasticHelpers.ReplaceNgram));
            Text(t => t.Name(n => n.UserTagNames).Analyzer(ElasticHelpers.Tag));
        }
    }
}