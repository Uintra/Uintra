using uIntra.Search.Core.Entities;
using uIntra.Search.Core.Queries;

namespace uIntra.Search.Core.Indexes
{
    public interface IElasticIndex
    {
        SearchResult<SearchableBase> Search(SearchTextQuery textQuery);

        void RecreateIndex();
    }
}