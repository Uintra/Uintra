using System;
using System.Collections.Generic;
using uIntra.Search.Core.Entities;

namespace uIntra.Search.Core.Indexes
{
    public interface IElasticActivityIndex
    {
        void Index(SearchableActivity activity);
        void Index(IEnumerable<SearchableActivity> activities);
        void Delete(Guid id);
        void DeleteByType(SearchableType type);
    }
}