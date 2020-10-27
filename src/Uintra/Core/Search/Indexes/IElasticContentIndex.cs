using System.Collections.Generic;
using Uintra.Core.Search.Entities;

namespace Uintra.Core.Search.Indexes
{
    public interface IElasticContentIndex
    {
        void Index(SearchableContent content);
        void Index(IEnumerable<SearchableContent> content);
        void Delete(int id);
    }
}
