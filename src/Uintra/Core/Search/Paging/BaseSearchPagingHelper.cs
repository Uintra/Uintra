using Nest;
using Uintra.Core.Search.Queries.SearchByText;

namespace Uintra.Core.Search.Paging
{
	public class BaseSearchPagingHelper<T> : ISearchPagingHelper<T> where T : class
	{
		public void Apply(SearchDescriptor<T> searchDescriptor, SearchByTextQuery query)
		{
			searchDescriptor.Skip(query.Skip).Take(query.Take);
		}
	}
}