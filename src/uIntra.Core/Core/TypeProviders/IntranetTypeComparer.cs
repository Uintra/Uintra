using System.Collections.Generic;

namespace uIntra.Core.TypeProviders
{
    public class IntranetTypeComparer : IEqualityComparer<IIntranetType>
    {
        public bool Equals(IIntranetType x, IIntranetType y)
        {
            return GetHashCode(x) == GetHashCode(y);
        }

        public int GetHashCode(IIntranetType obj)
        {
            return obj.Id ^ obj.Name.GetHashCode();
        }
    }
}
