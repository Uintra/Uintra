using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Uintra.Core.Utils
{
    public class HttpHelper : IDisposable, IHttpHelper
    {
        private readonly HttpClient _client = new HttpClient();

        public Task<byte[]> GetImageAsync(string uri) =>
            _client.GetByteArrayAsync(uri);

        public Task<string> GetStringAsync(string uri) =>
            _client.GetStringAsync(uri);

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
