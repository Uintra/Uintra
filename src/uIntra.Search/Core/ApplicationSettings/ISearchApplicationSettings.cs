using System.Collections.Generic;

namespace uIntra.Search
{
    public interface ISearchApplicationSettings
    {
        IEnumerable<string> IndexingDocumentTypesKey { get; }
    }
}
