using System.Collections.Generic;

namespace uIntra.Core.TypeProviders
{
    public class IntranetTypeComparer : IEqualityComparer<IIntranetType>
    {
        public bool Equals(IIntranetType x, IIntranetType y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(IIntranetType obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
