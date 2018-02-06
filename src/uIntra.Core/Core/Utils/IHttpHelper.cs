using System;
using System.Threading.Tasks;

namespace uIntra.Core.Utils
{
    public interface IHttpHelper
    {
        Task<byte[]> GetImageAsync(string uri);
        Task<string> GetStringAsync(string uri);
    }
}