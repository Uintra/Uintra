using System;
using System.Collections.Generic;
using Uintra.Core.TypeProviders;

namespace Uintra.Search
{
    public interface IElasticActivityIndex
    {
        void Index(SearchableActivity activity);
        void Index(IEnumerable<SearchableActivity> activities);
        void Delete(Guid id);
        void DeleteByType(Enum type);
    }
}