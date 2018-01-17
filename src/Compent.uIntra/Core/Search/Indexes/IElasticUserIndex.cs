using System;
using System.Collections.Generic;
using Compent.uIntra.Core.Search.Entities;

namespace Compent.uIntra.Core.Search.Indexes
{
    public interface IElasticUserIndex
    {
        void Delete(Guid id);
        void Index(IEnumerable<SearchableUser> users);
        void Index(SearchableUser user);
    }
}