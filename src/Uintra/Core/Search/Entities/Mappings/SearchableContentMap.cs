using System.Linq;
using Uintra.Core.Search.Helpers;

namespace Uintra.Core.Search.Entities.Mappings
{
    public class SearchableContentMap : SearchableBaseMap<SearchableContent>
    {
        public SearchableContentMap()
        {
            Text(t => t.Name(n => n.AggregatedTextFromPanels).Fielddata().Analyzer(ElasticHelpers.ReplaceNgram));
        }
    }
}