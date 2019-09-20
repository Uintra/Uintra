using System;

namespace Uintra.Core.TypeProviders
{
    public class MediaFolderTypeProvider : EnumTypeProviderBase, IMediaFolderTypeProvider
    {
        public MediaFolderTypeProvider(params Type[] enums) : base(enums)
        {

        }
    }
}
