using System.Collections.Generic;

namespace uIntra.Search.Core
{
    public interface ISearchApplicationSettings
    {
        IEnumerable<string> IndexingDocumentTypesKey { get; }
    }
}
