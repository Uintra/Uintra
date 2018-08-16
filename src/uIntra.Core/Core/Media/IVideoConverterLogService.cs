namespace Uintra.Core.Media
{
    public interface IVideoConverterLogService
    {
        void Log(bool result, string message, int mediaId);
    }
}