using System.Threading.Tasks;

namespace Uintra.Core.Utils
{
    public interface IHttpHelper
    {
        Task<byte[]> GetImageAsync(string uri);
        Task<string> GetStringAsync(string uri);
    }
}