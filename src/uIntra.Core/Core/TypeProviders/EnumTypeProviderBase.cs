using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Extensions;

namespace uIntra.Core.TypeProviders
{

    public abstract class EnumTypeProviderBase<T> : IEnumTypeProvider where T : struct
    {
        public virtual Enum Get(int typeId) => GetAll().Single(at => at.ToInt() == typeId);

        public virtual Enum Get(string name) => GetAll().Single(at => at.ToString() == name);

        public virtual IEnumerable<Enum> GetAll()
        {
            foreach (var enumRole in Enum.GetValues(typeof(T))) yield return (Enum)enumRole;
        }
    }
}
