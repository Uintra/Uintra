using System.Collections.Generic;
using System.Linq;

namespace Uintra.Core.Caching
{
    public class CachedList<T> : CachedItemBase
    {
        public IEnumerable<T> Items { get; set; }

        public CachedList()
        {
            Items = Enumerable.Empty<T>();
        }
    }
}