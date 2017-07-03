using System.Collections.Generic;

namespace uIntra.Core.TypeProviders
{
    public interface IIntranetTypeProvider
    {
        IIntranetType Get(int id);
        IIntranetType Get(string name);
        IEnumerable<IIntranetType> GetAll();
    }
}
