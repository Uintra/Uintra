using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Search.Indexers.Diagnostics.Models;

namespace Uintra.Features.Search.Models
{
    public class RebuildIndexStatusModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IEnumerable<IndexedModelResult> Index { get; set; } = Enumerable.Empty<IndexedModelResult>();
    }
}