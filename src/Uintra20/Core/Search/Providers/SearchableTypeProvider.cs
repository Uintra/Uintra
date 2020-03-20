using System;
using Uintra20.Infrastructure.TypeProviders;

namespace Uintra20.Core.Search.Providers
{
    public class SearchableTypeProvider : EnumTypeProviderBase, ISearchableTypeProvider
    {
        public SearchableTypeProvider(params Type[] enums) : base(enums)
        {

        }
    }
}
