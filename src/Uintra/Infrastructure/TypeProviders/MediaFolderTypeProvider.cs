using System;

namespace Uintra.Infrastructure.TypeProviders
{
    public class MediaFolderTypeProvider : EnumTypeProviderBase, IMediaFolderTypeProvider
    {
        public MediaFolderTypeProvider(params Type[] enums) : base(enums)
        {

        }
    }
}