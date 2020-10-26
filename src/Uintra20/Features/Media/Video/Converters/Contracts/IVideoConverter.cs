using Uintra20.Features.Media.Models;
using Umbraco.Core.Models;

namespace Uintra20.Features.Media.Video.Converters.Contracts
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
