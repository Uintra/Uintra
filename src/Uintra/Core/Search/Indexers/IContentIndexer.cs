using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UBaseline.Search.Core;

namespace Uintra.Core.Search.Indexers
{
    public interface IContentIndexer : ISearchDocumentIndexer
    {
        Task Index(int id);

        Task Delete(int id);
    }
}
