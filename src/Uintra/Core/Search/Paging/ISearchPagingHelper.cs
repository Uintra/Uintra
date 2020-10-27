using Nest;
using Uintra.Features.Search.Queries;

namespace Uintra.Core.Search.Paging
{
	public interface ISearchPagingHelper<T> where T : class
	{
		void Apply(SearchDescriptor<T> searchDescriptor, SearchTextQuery query);
	}
}
