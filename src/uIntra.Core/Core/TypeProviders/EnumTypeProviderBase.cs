using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Extensions;

namespace uIntra.Core.TypeProviders
{

    public abstract class EnumTypeProviderBase<T> : IEnumTypeProvider where T : struct
    {
        private IEnumerable<Enum> _all;

        public EnumTypeProviderBase()
        {
            _all = Enum.GetValues(typeof(T)).Cast<Enum>();
            TypeIdRelations = _all.ToDictionary(type => type.ToInt());
            TypeNameRelations = _all.ToDictionary(type => type.ToString());
        }

        public virtual IDictionary<int, Enum> TypeIdRelations { get; }

        public virtual IDictionary<string, Enum> TypeNameRelations { get; }

        public virtual Enum this[int typeId] => TypeIdRelations[typeId];

        public virtual Enum this[string name] => TypeNameRelations[name];

        public virtual IEnumerable<Enum> All => _all;
    }
}
