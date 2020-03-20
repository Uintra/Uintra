using Nest;
using Uintra20.Core.Search.Helpers;

namespace Uintra20.Core.Search.Entities.Mappings
{
    public class SearchableDocumentMap : SearchableBaseMap<SearchableDocument>
    {
        public SearchableDocumentMap()
        {
            Object<Attachment>(a => a.Name(n => n.Attachment).Properties(p => p.Text(t => t.Name(n => n.Content).Analyzer(ElasticHelpers.ReplaceNgram))));
        }
    }
}