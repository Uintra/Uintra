using System.Collections.Generic;
using Uintra20.Core.Search.Entities;

namespace Uintra20.Core.Search.Indexes
{
    public interface IElasticDocumentIndex
    {
        void Index(SearchableDocument content);
        void Index(IEnumerable<SearchableDocument> content);
        void Delete(int id);
    }
}
