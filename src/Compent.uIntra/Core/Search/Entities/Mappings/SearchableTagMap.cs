using Uintra.Search;

namespace Compent.Uintra.Core.Search.Entities.Mappings
{
    public class SearchableTagMap : SearchableBaseMap<SearchableTag>
    {
        public SearchableTagMap()
        {
            Text(t => t.Name(n => n.Title));
        }
    }
}