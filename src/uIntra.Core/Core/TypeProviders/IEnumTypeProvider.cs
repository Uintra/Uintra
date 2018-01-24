using System;
using System.Collections.Generic;

namespace uIntra.Core.TypeProviders
{
    public interface IEnumTypeProvider
    {
        Enum this[int typeId] { get; }

        Enum this[string name] { get; }

        List<Enum> All { get; }
    }
}