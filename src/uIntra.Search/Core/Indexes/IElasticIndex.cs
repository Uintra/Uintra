namespace Uintra.Search
{
    public interface IElasticIndex
    {
        SearchResult<SearchableBase> Search(SearchTextQuery query);

        bool RecreateIndex(out string error);
    }
}