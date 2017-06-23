namespace uIntra.Search.Core
{
    public interface IContentIndexer
    {
        void FillIndex(int id);

        void DeleteFromIndex(int id);
    }
}