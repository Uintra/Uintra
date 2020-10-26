using System.Collections.Generic;

namespace Uintra20.Core.Search.Indexers
{
    public interface IDocumentIndexer
    {
        void Index(int id);
        void Index(IEnumerable<int> ids);

        void DeleteFromIndex(int id);
        void DeleteFromIndex(IEnumerable<int> ids);
    }
}