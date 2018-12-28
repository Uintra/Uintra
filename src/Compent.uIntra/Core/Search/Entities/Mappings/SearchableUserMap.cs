using Nest;
using Uintra.Search;

namespace Compent.Uintra.Core.Search.Entities.Mappings
{
    public class SearchableUserMap : SearchableBaseMap<SearchableUser>
    {
        public SearchableUserMap()
        {
            Text(t => t.Name(n => n.Department).Fielddata().Analyzer(ElasticHelpers.ReplaceNgram));
            Text(t => t.Name(n => n.Phone).Fielddata().Analyzer(ElasticHelpers.ReplaceNgram));
            Text(t => t.Name(n => n.Email).Fielddata().Analyzer(ElasticHelpers.ReplaceNgram));
            Text(t => t.Name(n => n.FullName).Fielddata().Analyzer(ElasticHelpers.ReplaceNgram)
                .Fields(f => f
                    .Keyword(k => k
                        .Name(n => n.FullName.Suffix(ElasticHelpers.Normalizer.Sort))
                        .Normalizer(ElasticHelpers.Normalizer.Sort)
                    )
            ));
            Text(t => t.Name(n => n.UserTagNames).Analyzer(ElasticHelpers.Tag));
            Text(t => t.Name(n => n.GroupIds));
        }
    }
}