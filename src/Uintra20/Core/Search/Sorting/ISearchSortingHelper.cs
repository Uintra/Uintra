﻿using Nest;
using Uintra20.Features.Search.Queries;

namespace Uintra20.Core.Search.Sorting
{
	public interface ISearchSortingHelper<T> where T : class
	{
		void Apply(SearchDescriptor<T> searchDescriptor, SearchTextQuery query);
	}
}
