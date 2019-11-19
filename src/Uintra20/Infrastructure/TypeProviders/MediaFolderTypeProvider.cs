using System;

namespace Uintra20.Infrastructure.TypeProviders
{
    public class MediaFolderTypeProvider : EnumTypeProviderBase, IMediaFolderTypeProvider
    {
        public MediaFolderTypeProvider(params Type[] enums) : base(enums)
        {

        }
    }
}