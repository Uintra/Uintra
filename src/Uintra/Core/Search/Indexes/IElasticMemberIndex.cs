using System;
using System.Collections.Generic;
using Uintra.Core.Search.Entities;
using Uintra.Features.Search.Queries;

namespace Uintra.Core.Search.Indexes
{
	public interface IElasticMemberIndex<T> where T : SearchableMember
    {
		SearchResult<T> Search(MemberSearchQuery query);
		void Delete(Guid id);
		void Index(IEnumerable<T> members);
		void Index(T member);
	}
}
