using System.Collections.Generic;
using System.Linq;

namespace uIntra.Core.Extentions
{
    public static class ObjectExtensions
    {
        public static bool In<T>(this T value, params T[] items)
        {
            if (items == null || (object)value == null)
                return false;
            return ((IEnumerable<T>)items).Contains<T>(value);
        }
    }
}
