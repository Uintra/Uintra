using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Extensions;
using static LanguageExt.Prelude;

namespace Uintra.Core.TypeProviders
{
    public abstract class EnumTypeProviderBase<T> : IEnumTypeProvider where T : struct
    {
        protected EnumTypeProviderBase()
        {
            All = Enum.GetValues(typeof(T)).Cast<Enum>().ToArray();

            IntTypeDictionary = All.ToDictionary(EnumExtensions.ToInt, identity);
            StringTypeDictionary = All.ToDictionary(toString, identity);
        }

        public Enum[] All { get; }

        public IDictionary<int, Enum> IntTypeDictionary { get; }

        public IDictionary<string, Enum> StringTypeDictionary { get; }

        public virtual Enum this[int typeId] => IntTypeDictionary[typeId];

        public virtual Enum this[string typeName] => StringTypeDictionary[typeName];
    }
}
