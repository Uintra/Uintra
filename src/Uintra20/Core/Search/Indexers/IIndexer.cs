using Uintra20.Core.Search.Indexers.Diagnostics.Models;

namespace Uintra20.Core.Search.Indexers
{
    public interface IIndexer
    {
        IndexedModelResult FillIndex();
    }
}