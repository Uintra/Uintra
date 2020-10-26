using System;
using System.Collections.Generic;
using System.Linq;

namespace Uintra.Infrastructure.Helpers
{
    public static class EnumHelper
    {
        public static IEnumerable<T> ToEnumerable<T>() => 
            Enum.GetValues(typeof(T)).Cast<T>();
    }
}