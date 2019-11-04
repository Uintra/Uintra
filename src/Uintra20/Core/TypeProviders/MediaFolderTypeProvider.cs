using System;

namespace Uintra20.Core.TypeProviders
{
    public class MediaFolderTypeProvider : EnumTypeProviderBase, IMediaFolderTypeProvider
    {
        public MediaFolderTypeProvider(params Type[] enums) : base(enums)
        {

        }
    }
}