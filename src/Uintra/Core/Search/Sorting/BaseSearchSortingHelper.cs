using Compent.Extensions;
using Nest;
using Uintra.Core.Search.Helpers;
using Uintra.Core.Search.Queries.SearchByText;

namespace Uintra.Core.Search.Sorting
{
	public class BaseSearchSortingHelper<T> : ISearchSortingHelper<T> where T : class
	{
		public virtual void Apply(SearchDescriptor<T> searchDescriptor, SearchByTextQuery query)
		{
			SortByName(searchDescriptor, query.OrderingString);
		}

		protected virtual void SortByName(SearchDescriptor<T> searchDescriptor, string propertyName = "_score") 
		{
			if (propertyName.In(ElasticHelpers.FullName))
			{
				propertyName += $".{ElasticHelpers.Normalizer.Sort}";
				searchDescriptor.Sort(s => s.Ascending(propertyName));
			}
			else
			{
				searchDescriptor.Sort(s => s.Descending(propertyName));
			}
		}
	}
}