using System;
using System.Collections.Generic;
using System.Linq;
using Uintra20.Core.Search.Indexers.Diagnostics.Models;

namespace Uintra20.Core.Search.Indexers.Diagnostics
{
    public class IndexerDiagnosticService : IIndexerDiagnosticService
    {
        public IndexerDiagnosticService()
        {
        }

        public IndexedModelResult GetFailedResult(
            string message,
            string indexName
        ) =>
            new IndexedModelResult
            {
                Success = false,
                Message = message,
                IndexedName = indexName,
                IndexedItems = 0
            };

        public IndexedModelResult GetSuccessResult<T>(
            string indexName,
            IEnumerable<T> items
        ) =>
            new IndexedModelResult
            {
                Success = true,
                IndexedName = indexName,
                IndexedItems = items.Count(),
                Message = string.Empty
            };
    }
}