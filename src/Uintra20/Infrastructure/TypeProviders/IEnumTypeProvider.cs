using System;
using System.Collections.Generic;

namespace Uintra20.Infrastructure.TypeProviders
{
    public interface IEnumTypeProvider
    {
        Enum this[int typeId] { get; }

        Enum this[string name] { get; }

        Enum[] All { get; }

        IDictionary<int, Enum> IntTypeDictionary { get; }

        IDictionary<string, Enum> StringTypeDictionary { get; }
    }
}
