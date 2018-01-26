using uIntra.Search;

namespace Compent.uIntra.Core.Search.Entities.Mappings
{
    public class SearchableTagMap : SearchableBaseMap<SearchableTag>
    {
        public SearchableTagMap()
        {
            Text(t => t.Name(n => n.Title).Analyzer(ElasticHelpers.ReplaceNgram));
        }
    }
}