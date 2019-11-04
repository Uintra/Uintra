using System;
using System.Collections.Generic;
using Compent.Extensions;

namespace Uintra20.Core.Extensions
{
    public static class EnumTypeExtensions
    {
        public static Enum Get(this IEnumerable<Enum> types, string typeName) =>
            types
                .AsList()
                .Find(type => type.ToString() == typeName);

        public static Enum Get(this IEnumerable<Enum> types, int typeId) =>
            types
                .AsList()
                .Find(type => type.ToInt() == typeId);
    }
}