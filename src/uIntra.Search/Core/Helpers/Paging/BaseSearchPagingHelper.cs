using Nest;

namespace Uintra.Search.Paging
{
	public class BaseSearchPagingHelper<T> : ISearchPagingHelper<T> where T : class
	{
		public void Apply(SearchDescriptor<T> searchDescriptor, SearchTextQuery query)
		{
			searchDescriptor.Skip(query.Skip).Take(query.Take);
		}
	}
}