using System;
using System.Collections.Generic;
using Uintra20.Features.Search.Entities;
using Uintra20.Features.Search.Queries;

namespace Uintra20.Features.Search.Indexes
{
	public interface IElasticMemberIndex<T> where T : SearchableMember
    {
		SearchResult<T> Search(MemberSearchQuery query);
		void Delete(Guid id);
		void Index(IEnumerable<T> members);
		void Index(T member);
	}
}
