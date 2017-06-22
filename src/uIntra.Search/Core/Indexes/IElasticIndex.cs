namespace uIntra.Search.Core
{
    public interface IElasticIndex
    {
        SearchResult<SearchableBase> Search(SearchTextQuery textQuery);

        void RecreateIndex();
    }
}