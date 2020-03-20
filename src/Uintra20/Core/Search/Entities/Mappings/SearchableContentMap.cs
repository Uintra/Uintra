using Uintra20.Core.Search.Helpers;

namespace Uintra20.Core.Search.Entities.Mappings
{
    public class SearchableContentMap : SearchableBaseMap<SearchableContent>
    {
        public SearchableContentMap()
        {
            Nested<SearchablePanel>(nst =>
                nst.Name(n => n.Panels)
                    .Properties(p => p.Text(t => t.Name(n => n.Content).Analyzer(ElasticHelpers.ReplaceNgram))));

            Nested<SearchablePanel>(nst =>
                nst.Name(n => n.Panels)
                    .Properties(p => p.Text(t => t.Name(n => n.Title).Analyzer(ElasticHelpers.ReplaceNgram))));
        }
    }
}