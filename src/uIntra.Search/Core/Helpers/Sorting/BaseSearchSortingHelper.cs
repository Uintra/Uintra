using Compent.Extensions;
using Nest;

namespace Uintra.Search.Sorting
{
	public class BaseSearchSortingHelper<T> : ISearchSortingHelper<T> where T : class
	{
		public virtual void Apply(SearchDescriptor<T> searchDescriptor, SearchTextQuery query)
		{
			SortByName(searchDescriptor);
		}

		protected virtual void SortByName(SearchDescriptor<T> searchDescriptor, string propertyName = "_score") 
		{
			if (propertyName.In("fullName"))
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