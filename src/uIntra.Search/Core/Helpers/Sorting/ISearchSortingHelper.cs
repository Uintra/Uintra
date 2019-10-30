using Nest;

namespace Uintra.Search.Sorting
{
	public interface ISearchSortingHelper<T> where T : class
	{
		void Apply(SearchDescriptor<T> searchDescriptor, SearchTextQuery query);
	}
}
