using System.Collections.Generic;
using Compent.uIntra.Core.Search.Entities;

namespace Compent.uIntra.Core.Search.Indexes
{
    public interface IElasticUintraContentIndex
    {
        void Delete(int id);
        SearchableUintraContent Get(int id);
        void Index(IEnumerable<SearchableUintraContent> activities);
        void Index(SearchableUintraContent activity);
    }
}