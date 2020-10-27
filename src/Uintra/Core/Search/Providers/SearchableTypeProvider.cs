using System;
using Uintra.Infrastructure.TypeProviders;

namespace Uintra.Core.Search.Providers
{
    public class SearchableTypeProvider : EnumTypeProviderBase, ISearchableTypeProvider
    {
        public SearchableTypeProvider(params Type[] enums) : base(enums)
        {

        }
    }
}
