namespace uIntra.Search
{
    public interface IElasticIndex
    {
        SearchResult<SearchableBase> Search(SearchTextQuery textQuery);

        void RecreateIndex();
    }
}