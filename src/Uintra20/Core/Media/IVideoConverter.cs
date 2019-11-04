using Umbraco.Core.Models;

namespace Uintra20.Core.Media
{
    public interface IVideoConverter
    {
        void Convert(MediaConvertModel model);
        bool NeedConvert(string mediaTypeAlias, string fileName);
        bool IsConverting(IMedia media);
        bool IsMp4(string filename);
        bool IsVideo(string filename);
    }
}
