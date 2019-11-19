using System;
using System.Collections.Generic;

namespace Uintra.Search
{
	public interface IElasticMemberIndex<T> where T : SearchableMember
    {
		SearchResult<T> Search(MemberSearchQuery query);
		void Delete(Guid id);
		void Index(IEnumerable<T> members);
		void Index(T member);
	}
}
