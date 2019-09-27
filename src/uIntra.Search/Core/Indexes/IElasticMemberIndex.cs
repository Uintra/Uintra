using System;
using System.Collections.Generic;

namespace Uintra.Search
{
	public interface IElasticMemberIndex
	{
		SearchResult<SearchableMember> Search(MemberSearchQuery query);
		void Delete(Guid id);
		void Index(IEnumerable<SearchableMember> members);
		void Index(SearchableMember member);
	}
}
