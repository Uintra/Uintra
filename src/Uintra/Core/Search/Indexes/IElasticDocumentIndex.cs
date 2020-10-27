using System.Collections.Generic;
using Uintra.Core.Search.Entities;

namespace Uintra.Core.Search.Indexes
{
    public interface IElasticDocumentIndex
    {
        void Index(SearchableDocument content);
        void Index(IEnumerable<SearchableDocument> content);
        void Delete(int id);
    }
}
