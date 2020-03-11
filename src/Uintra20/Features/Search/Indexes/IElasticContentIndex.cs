using System.Collections.Generic;
using Uintra20.Features.Search.Entities;

namespace Uintra20.Features.Search.Indexes
{
    public interface IElasticContentIndex
    {
        void Index(SearchableContent content);
        void Index(IEnumerable<SearchableContent> content);
        void Delete(int id);
    }
}
