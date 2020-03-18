using System.IO;

namespace Uintra20.Features.Media.Images.Helpers.Contracts
{
    public interface IImageHelper
    {
        MemoryStream NormalizeOrientation(Stream imageStream, string imageExtension, bool removeExifOrientationTag = true);
        bool IsFileImage(byte[] fileBytes);
        string GetImageWithPreset(string source, string preset);
        string GetImageWithResize(string source, string resize);
    }
}
