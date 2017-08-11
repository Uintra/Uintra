using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uIntra.Core.Media
{
    public interface IImageHelper
    {
        MemoryStream NormalizeOrientation(Stream imageStream, string imageExtension, bool removeExifOrientationTag = true);
        bool IsFileImage(byte[] fileBytes);
    }
}
