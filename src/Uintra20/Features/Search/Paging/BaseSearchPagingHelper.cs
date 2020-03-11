using Nest;
using Uintra20.Features.Search.Queries;

namespace Uintra20.Features.Search.Paging
{
	public class BaseSearchPagingHelper<T> : ISearchPagingHelper<T> where T : class
	{
		public void Apply(SearchDescriptor<T> searchDescriptor, SearchTextQuery query)
		{
			searchDescriptor.Skip(query.Skip).Take(query.Take);
		}
	}
}