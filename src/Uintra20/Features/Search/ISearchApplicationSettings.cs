using System.Collections.Generic;

namespace Uintra20.Features.Search
{
    public interface ISearchApplicationSettings
    {
        IEnumerable<string> IndexingDocumentTypesKey { get; }
    }
}
