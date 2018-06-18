using System;
using System.Collections.Generic;
using System.Linq;

namespace uIntra.Core.TypeProviders
{
    public abstract class IntranetTypeProviderBase<T> : IIntranetTypeProvider
    {
        public virtual IIntranetType Get(int typeId)
        {
            // TODO: Use .Cast?
            return GetAll().Single(at => at.Id == typeId);
        }
        public virtual IIntranetType Get(string name)
        {
            // TODO: Use .Cast?
            return GetAll().Single(at => at.Name == name);
        }

        public virtual IEnumerable<IIntranetType> GetAll()
        {
            foreach (var enumRole in Enum.GetValues(typeof(T)))
            {
                yield return new IntranetType
                {
                    Id = (int)enumRole,
                    Name = enumRole.ToString()
                };
            }
        }
    }
}
