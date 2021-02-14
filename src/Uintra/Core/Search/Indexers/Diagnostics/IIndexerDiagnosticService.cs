using System.Collections.Generic;
using Uintra.Core.Search.Indexers.Diagnostics.Models;

namespace Uintra.Core.Search.Indexers.Diagnostics
{
    public interface IIndexerDiagnosticService
    {
        IndexedModelResult GetFailedResult(string message, string indexName);
        IndexedModelResult GetSuccessResult<T>(string indexName, IEnumerable<T> items);
    }
}