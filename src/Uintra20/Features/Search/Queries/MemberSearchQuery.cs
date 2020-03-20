using System;

namespace Uintra20.Features.Search.Queries
{
	public class MemberSearchQuery:SearchTextQuery
	{
		public Guid? GroupId { get; set; }
		public bool MembersOfGroup { get; set; }
	}
}
