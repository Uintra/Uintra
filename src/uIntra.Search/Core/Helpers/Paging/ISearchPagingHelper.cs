using Nest;

namespace Uintra.Search.Paging
{
	public interface ISearchPagingHelper<T> where T : class
	{
		void Apply(SearchDescriptor<T> searchDescriptor, SearchTextQuery query);
	}
}
