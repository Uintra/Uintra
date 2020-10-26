using Uintra20.Core.Search.Helpers;

namespace Uintra20.Core.Search.Entities.Mappings
{
    public class SearchableActivityMap : SearchableBaseMap<SearchableActivity>
    {
        public SearchableActivityMap()
        {
            Text(t => t.Name(n => n.Description).Analyzer(ElasticHelpers.ReplaceNgram));
        }
    }
}