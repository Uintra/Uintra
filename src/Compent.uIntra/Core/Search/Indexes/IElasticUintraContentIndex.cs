using System.Collections.Generic;
using Compent.Uintra.Core.Search.Entities;

namespace Compent.Uintra.Core.Search.Indexes
{
    public interface IElasticUintraContentIndex
    {
        void Delete(int id);
        SearchableUintraContent Get(int id);
        void Index(IEnumerable<SearchableUintraContent> activities);
        void Index(SearchableUintraContent activity);
    }
}