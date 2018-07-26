namespace Uintra.Core.Media
{
    public interface IVideoConverter
    {
        void Convert(MediaConvertModel model);
        bool NeedConvert(string mediaTypeAlias, string fileName);
    }
}