using Uintra20.Core.Search.Entities;
using Uintra20.Features.Search.Queries;

namespace Uintra20.Core.Search.Indexes
{
    public interface IElasticIndex
    {
        SearchResult<SearchableBase> Search(SearchTextQuery query);

        bool RecreateIndex(out string error);
    }
}