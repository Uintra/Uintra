using Uintra.Core.Search.Entities;
using Uintra.Features.Search.Queries;

namespace Uintra.Core.Search.Indexes
{
    public interface IElasticIndex
    {
        SearchResult<SearchableBase> Search(SearchTextQuery query);

        bool RecreateIndex(out string error);
    }
}