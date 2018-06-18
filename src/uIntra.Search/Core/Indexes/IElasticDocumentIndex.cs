using System.Collections.Generic;

namespace Uintra.Search
{
    public interface IElasticDocumentIndex
    {
        void Index(SearchableDocument content);
        void Index(IEnumerable<SearchableDocument> content);
        void Delete(int id);
    }
}
