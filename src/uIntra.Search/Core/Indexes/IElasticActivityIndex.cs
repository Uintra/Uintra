using System;
using System.Collections.Generic;

namespace uIntra.Search.Core
{
    public interface IElasticActivityIndex
    {
        void Index(SearchableActivity activity);
        void Index(IEnumerable<SearchableActivity> activities);
        void Delete(Guid id);
        void DeleteByType(SearchableType type);
    }
}