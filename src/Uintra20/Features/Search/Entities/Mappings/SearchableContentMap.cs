namespace Uintra20.Features.Search.Entities.Mappings
{
    public class SearchableContentMap : SearchableBaseMap<SearchableContent>
    {
        public SearchableContentMap()
        {
            Text(t => t.Name(n => n.PanelContent).Analyzer(ElasticHelpers.ReplaceNgram));
            Text(t => t.Name(n => n.PanelTitle).Analyzer(ElasticHelpers.ReplaceNgram));
        }
    }
}