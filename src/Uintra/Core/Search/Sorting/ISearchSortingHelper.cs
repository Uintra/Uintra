using Nest;
using Uintra.Core.Search.Queries;

namespace Uintra.Core.Search.Sorting
{
	public interface ISearchSortingHelper<T> where T : class
	{
		void Apply(SearchDescriptor<T> searchDescriptor, SearchByTextQuery query);
	}
}
