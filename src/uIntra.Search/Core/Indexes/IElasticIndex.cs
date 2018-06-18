namespace Uintra.Search
{
    public interface IElasticIndex
    {
        SearchResult<SearchableBase> Search(SearchTextQuery query);

        void RecreateIndex();
    }
}