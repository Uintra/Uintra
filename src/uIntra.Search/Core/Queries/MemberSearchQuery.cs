using System;

namespace Uintra.Search
{
	public class MemberSearchQuery:SearchTextQuery
	{
		public Guid? GroupId { get; set; }
		public bool MembersOfGroup { get; set; }
	}
}
