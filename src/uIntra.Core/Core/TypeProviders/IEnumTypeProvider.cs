using System;
using System.Collections.Generic;

namespace uIntra.Core.TypeProviders
{
    public interface IEnumTypeProvider
    {
        IDictionary<int, Enum> TypeIdRelations { get; }

        IDictionary<string, Enum> TypeNameRelations { get; }

        Enum this[int typeId] { get; }

        Enum this[string name] { get; }

        IEnumerable<Enum> All { get; }
    }
}