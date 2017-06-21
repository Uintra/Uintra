using System.Collections.Generic;
using uIntra.Search.Core.Entities;

namespace uIntra.Search.Core.Indexes
{
    public interface IElasticContentIndex
    {
        void Index(SearchableContent content);
        void Index(IEnumerable<SearchableContent> content);
        void Delete(int id);
    }
}
