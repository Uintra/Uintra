namespace Uintra20.Features.Search.Entities.Mappings
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