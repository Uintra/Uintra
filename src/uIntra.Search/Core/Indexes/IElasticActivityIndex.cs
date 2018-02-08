using System;
using System.Collections.Generic;
using uIntra.Core.TypeProviders;

namespace uIntra.Search
{
    public interface IElasticActivityIndex
    {
        void Index(SearchableActivity activity);
        void Index(IEnumerable<SearchableActivity> activities);
        void Delete(Guid id);
        void DeleteByType(Enum type);
    }
}