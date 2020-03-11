using System.Collections.Generic;
using Uintra20.Features.Search.Entities;

namespace Uintra20.Features.Search.Indexes
{
    public interface IElasticDocumentIndex
    {
        void Index(SearchableDocument content);
        void Index(IEnumerable<SearchableDocument> content);
        void Delete(int id);
    }
}
