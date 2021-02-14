using System.Collections.Generic;
using System.Threading.Tasks;
using UBaseline.Search.Core;

namespace Uintra.Core.Search.Indexers
{
    public interface IDocumentIndexer : ISearchDocumentIndexer
    {
        Task<int> Index(int id);
        Task<int> Index(IEnumerable<int> ids);

        Task<bool> DeleteFromIndex(int id);
        Task<bool> DeleteFromIndex(IEnumerable<int> ids);
    }
}
