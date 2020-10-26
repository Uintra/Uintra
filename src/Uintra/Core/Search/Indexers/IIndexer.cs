using Uintra.Core.Search.Indexers.Diagnostics.Models;

namespace Uintra.Core.Search.Indexers
{
    public interface IIndexer
    {
        IndexedModelResult FillIndex();
    }
}