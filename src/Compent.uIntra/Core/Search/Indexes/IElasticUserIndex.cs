using System;
using System.Collections.Generic;
using Compent.Uintra.Core.Search.Entities;

namespace Compent.Uintra.Core.Search.Indexes
{
    public interface IElasticUserIndex
    {
        void Delete(Guid id);
        void Index(IEnumerable<SearchableUser> users);
        void Index(SearchableUser user);
    }
}