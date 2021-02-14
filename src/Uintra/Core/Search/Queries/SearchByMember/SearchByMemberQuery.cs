using System;
using Compent.Shared.Search.Contract;
using Uintra.Core.Search.Entities;

namespace Uintra.Core.Search.Queries
{
    public class SearchByMemberQuery : SearchByTextQuery, ISearchQuery<SearchableMember>
    {
        public Guid? GroupId { get; set; }
        public bool MembersOfGroup { get; set; }
    }
}