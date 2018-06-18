namespace Uintra.Search
{
    public interface IContentIndexer
    {
        void FillIndex(int id);

        void DeleteFromIndex(int id);
    }
}