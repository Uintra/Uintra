using System.Collections.Generic;

namespace uIntra.Search.Core
{
    public interface IElasticDocumentIndex
    {
        void Index(SearchableDocument content);
        void Index(IEnumerable<SearchableDocument> content);
        void Delete(int id);
    }
}
