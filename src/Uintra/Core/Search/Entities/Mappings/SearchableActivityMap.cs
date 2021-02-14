using Uintra.Core.Search.Helpers;

namespace Uintra.Core.Search.Entities.Mappings
{
    public class SearchableActivityMap : SearchableBaseMap<SearchableActivity>
    {
        public SearchableActivityMap()
        {
            Text(t => t.Name(n => n.Description).Analyzer(ElasticHelpers.ReplaceNgram));
            Text(t => t.Name(n => n.UserTagNames).Analyzer(ElasticHelpers.Tag));
        }
    }
}