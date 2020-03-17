using System;
using System.Collections.Generic;
using Uintra20.Core.Search.Entities;

namespace Uintra20.Core.Search.Indexes
{
    public interface IElasticUintraActivityIndex
    {
        SearchableUintraActivity Get(Guid id);
        void Index(SearchableUintraActivity activity);
        void Index(IEnumerable<SearchableUintraActivity> activities);
        void Delete(Guid id);
        void DeleteByType(Enum type);
    }
}