using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Compent.Shared.Search.Contract;
using Uintra.Core.Search.Entities;
using Uintra.Core.Search.Queries.SearchByText;

namespace Uintra.Core.Search.Queries.SearchByMember
{
    public class SearchByMemberQuery : SearchByTextQuery, ISearchQuery<SearchableMember>
    {
        public Guid? GroupId { get; set; }
        public bool MembersOfGroup { get; set; }
    }
}