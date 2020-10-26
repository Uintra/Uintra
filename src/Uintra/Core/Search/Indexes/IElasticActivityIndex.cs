using System;
using System.Collections.Generic;
using Uintra.Core.Search.Entities;

namespace Uintra.Core.Search.Indexes
{
    public interface IElasticActivityIndex
    {
        void Index(SearchableActivity activity);
        void Index(IEnumerable<SearchableActivity> activities);
        void Delete(Guid id);
        void DeleteByType(Enum type);
    }
}