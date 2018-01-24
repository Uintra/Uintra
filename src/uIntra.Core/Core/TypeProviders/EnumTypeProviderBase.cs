using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Extensions;

namespace uIntra.Core.TypeProviders
{

    public abstract class EnumTypeProviderBase<T> : IEnumTypeProvider where T : struct
    {
        private List<Enum> _all;

        public EnumTypeProviderBase()
        {
            _all = Enum.GetValues(typeof(T)).Cast<Enum>().ToList();
        }

        public virtual Enum this[int typeId] => All.Get(typeId);

        public virtual Enum this[string typeName] => All.Get(typeName);

        public virtual List<Enum> All => _all;
    }

    public static class EnumTypeProvider
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
