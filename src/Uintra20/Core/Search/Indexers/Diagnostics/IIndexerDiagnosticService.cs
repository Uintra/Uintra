using System;
using System.Collections.Generic;
using Uintra20.Core.Search.Indexers.Diagnostics.Models;

namespace Uintra20.Core.Search.Indexers.Diagnostics
{
    public interface IIndexerDiagnosticService
    {
        IndexedModelResult GetFailedResult(string message, string indexName);
        IndexedModelResult GetSuccessResult<T>(string indexName, IEnumerable<T> items);
    }
}