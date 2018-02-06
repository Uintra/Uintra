using System;
using System.Collections.Generic;
using Extensions;
using uIntra.Core.Extensions;

namespace uIntra.Core.TypeProviders
{
    public static class EnumTypeProviderExtensions
    {
        public static Enum Get(this IEnumerable<Enum> types, string typeName) =>
            types
                .AsList()
                .Find(type => type.ToString().Equals(typeName));

        public static Enum Get(this IEnumerable<Enum> types, int typeId) =>
            types
                .AsList()
                .Find(type => type.ToInt().Equals(typeId));
    }
}