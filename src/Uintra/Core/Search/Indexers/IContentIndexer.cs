namespace Uintra.Core.Search.Indexers
{
    public interface IContentIndexer
    {
        void FillIndex(int id);

        void DeleteFromIndex(int id);
    }
}