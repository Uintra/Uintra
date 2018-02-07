using uIntra.Search;

namespace Compent.uIntra.Core.Search.Entities.Mappings
{
    public class SearchableUserMap : SearchableBaseMap<SearchableUser>
    {
        public SearchableUserMap()
        {
            Text(t => t.Name(n => n.Email).Analyzer(ElasticHelpers.ReplaceNgram));
            Text(t => t.Name(n => n.FullName).Analyzer(ElasticHelpers.ReplaceNgram));
            Text(t => t.Name(n => n.UserTagNames).Analyzer(ElasticHelpers.ReplaceNgram));
        }
    }
}