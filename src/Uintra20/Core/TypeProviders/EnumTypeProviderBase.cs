using System;
using System.Collections.Generic;
using System.Linq;
using Uintra20.Core.Extensions;
using static LanguageExt.Prelude;

namespace Uintra20.Core.TypeProviders
{
    public abstract class EnumTypeProviderBase : IEnumTypeProvider
    {
        protected EnumTypeProviderBase(params Type[] enums)
        {
            All = enums.SelectMany(e => Enum.GetValues(e).Cast<Enum>()).ToArray();
            IntTypeDictionary = All.ToDictionary(EnumExtensions.ToInt);
            StringTypeDictionary = All.ToDictionary(toString);
        }

        public Enum[] All { get; }

        public IDictionary<int, Enum> IntTypeDictionary { get; }

        public IDictionary<string, Enum> StringTypeDictionary { get; }

        public virtual Enum this[int typeId] => IntTypeDictionary[typeId];

        public virtual Enum this[Enum type] => IntTypeDictionary[type.ToInt()];

        public virtual Enum this[string typeName] => StringTypeDictionary[typeName];
    }
}