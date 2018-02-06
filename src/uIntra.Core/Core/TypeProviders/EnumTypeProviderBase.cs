using System;
using System.Collections.Generic;
using System.Linq;

namespace uIntra.Core.TypeProviders
{

    public abstract class EnumTypeProviderBase<T> : IEnumTypeProvider where T : struct
    {
        public EnumTypeProviderBase()
        {
            All = Enum.GetValues(typeof(T)).Cast<Enum>().ToList();
        }

        public virtual Enum this[int typeId] => All.Get(typeId);

        public virtual Enum this[string typeName] => All.Get(typeName);

        public virtual List<Enum> All { get; }
    }
}
