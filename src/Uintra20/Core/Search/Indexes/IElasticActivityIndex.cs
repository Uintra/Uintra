using System;
using System.Collections.Generic;
using Uintra20.Core.Search.Entities;

namespace Uintra20.Core.Search.Indexes
{
    public interface IElasticActivityIndex
    {
        void Index(SearchableActivity activity);
        void Index(IEnumerable<SearchableActivity> activities);
        void Delete(Guid id);
        void DeleteByType(Enum type);
    }
}