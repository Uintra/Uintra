using System.Collections.Generic;
using Uintra20.Features.Search.Entities;

namespace Uintra20.Features.Search.Indexes
{
    public interface IElasticUintraContentIndex
    {
        void Delete(int id);
        SearchableUintraContent Get(int id);
        void Index(IEnumerable<SearchableUintraContent> activities);
        void Index(SearchableUintraContent activity);
    }
}