using System;
using Uintra.Core.TypeProviders;

namespace Uintra.Search
{
    public class SearchableTypeProvider : EnumTypeProviderBase, ISearchableTypeProvider
    {
        public SearchableTypeProvider(params Type[] enums) : base(enums)
        {

        }
    }
}
