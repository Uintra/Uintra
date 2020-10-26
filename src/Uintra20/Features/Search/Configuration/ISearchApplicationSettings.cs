using System.Collections.Generic;

namespace Uintra20.Features.Search.Configuration
{
    public interface ISearchApplicationSettings
    {
        IEnumerable<string> IndexingDocumentTypesKey { get; }
    }
}
