﻿using Nest;
using Uintra.Core.Search.Queries;

namespace Uintra.Core.Search.Paging
{
	public interface ISearchPagingHelper<T> where T : class
	{
		void Apply(SearchDescriptor<T> searchDescriptor, SearchByTextQuery query);
	}
}
