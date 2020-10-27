using System.Collections.Generic;

namespace Uintra.Features.Search.Configuration
{
    public interface ISearchApplicationSettings
    {
        IEnumerable<string> IndexingDocumentTypesKey { get; }
    }
}
