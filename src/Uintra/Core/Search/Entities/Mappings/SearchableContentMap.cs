using System.Linq;
using Uintra.Core.Search.Helpers;

namespace Uintra.Core.Search.Entities.Mappings
{
    public class SearchableContentMap : SearchableBaseMap<SearchableContent>
    {
        public SearchableContentMap()
        {
            Nested<SearchablePanel>(nst =>
                nst.Name(n => n.Panels.First())
                    .Properties(p => p.Text(t => t.Name(n => n.Content).Fielddata().Analyzer(ElasticHelpers.ReplaceNgram))));

            Nested<SearchablePanel>(nst =>
                nst.Name(n => n.Panels.First())
                    .Properties(p => p.Text(t => t.Name(n => n.Title).Fielddata().Analyzer(ElasticHelpers.ReplaceNgram))));
        }
    }
}