namespace Uintra20.Features.Search
{
    public interface IContentIndexer
    {
        void FillIndex(int id);

        void DeleteFromIndex(int id);
    }
}