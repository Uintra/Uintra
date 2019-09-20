using System;
using Uintra.Core.TypeProviders;

namespace Uintra.Core
{
    public class ContextTypeProvider : EnumTypeProviderBase, IContextTypeProvider
    {
        public ContextTypeProvider(params Type[] enums) : base(enums)
        {

        }
    }
}
