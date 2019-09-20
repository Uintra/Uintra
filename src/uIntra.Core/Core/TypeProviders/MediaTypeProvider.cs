using System;

namespace Uintra.Core.TypeProviders
{
    public class MediaTypeProvider : EnumTypeProviderBase, IMediaTypeProvider
    {
        public MediaTypeProvider(params Type[] enums) : base(enums)
        {

        }
    }
}
