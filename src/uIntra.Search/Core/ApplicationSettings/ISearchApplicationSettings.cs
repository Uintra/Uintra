using System.Collections.Generic;

namespace Uintra.Search
{
    public interface ISearchApplicationSettings
    {
        IEnumerable<string> IndexingDocumentTypesKey { get; }
    }
}
