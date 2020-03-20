using System.Collections.Generic;
using Uintra20.Core.Search.Entities;

namespace Uintra20.Core.Search.Indexes
{
    public interface IElasticContentIndex
    {
        void Index(SearchableContent content);
        void Index(IEnumerable<SearchableContent> content);
        void Delete(int id);
    }
}
