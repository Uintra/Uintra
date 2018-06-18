using System.IO;

namespace Uintra.Core.Extensions
{
    public static class StreamExtensions
    {
        public static byte[] ToBytes(this Stream stream)
        {
            var ms = new MemoryStream();
            stream.CopyTo(ms);
            var result = ms.ToArray();

            ms.Dispose();
            return result;
        }
    }
}
