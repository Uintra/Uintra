using System.Threading.Tasks;

namespace Uintra20.Core.Media
{
    public interface IVideoConverterLogService
    {
        void Log(bool result, string message, int mediaId);
        Task LogAsync(bool result, string message, int mediaId);
    }
}