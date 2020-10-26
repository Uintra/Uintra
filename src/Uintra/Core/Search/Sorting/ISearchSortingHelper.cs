using Nest;
using Uintra.Features.Search.Queries;

namespace Uintra.Core.Search.Sorting
{
	public interface ISearchSortingHelper<T> where T : class
	{
		void Apply(SearchDescriptor<T> searchDescriptor, SearchTextQuery query);
	}
}
