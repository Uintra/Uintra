using System;
using System.Collections.Generic;

namespace uIntra.Core.TypeProviders
{
    public interface IEnumTypeProvider
    {
        Enum Get(int typeId);
        Enum Get(string name);
        IEnumerable<Enum> GetAll();
    }
}