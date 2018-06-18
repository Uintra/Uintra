using System.Collections.Generic;

namespace Uintra.Search
{
    public interface IElasticContentIndex
    {
        void Index(SearchableContent content);
        void Index(IEnumerable<SearchableContent> content);
        void Delete(int id);
    }
}
