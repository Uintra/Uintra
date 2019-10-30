using System;
using Uintra.Core.TypeProviders;
using Uintra.Search;

namespace Compent.Uintra.Core.Search
{
    public class UintraSearchableTypeProvider : EnumTypeProviderBase, ISearchableTypeProvider
    {
        public UintraSearchableTypeProvider(params Type[] enums) : base(enums)
        {

        }
    }
}
