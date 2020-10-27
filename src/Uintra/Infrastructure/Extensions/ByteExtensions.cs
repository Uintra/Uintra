namespace Uintra.Infrastructure.Extensions
{
    public static class ByteExtensions
    {
        public static float ToMegabytes(this byte[] source) => 
            source.Length / 1024.0F / 1024.0F;
    }
}