using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Uintra20.Infrastructure.Utils
{
    public static class EmbeddedResourcesUtils
    {
        public static string ReadResourceContent(string embeddedResourceName)
        {
            var assembly = Assembly.GetCallingAssembly();
            return ReadResourceContent(embeddedResourceName, assembly);
        }

        public static async Task<string> ReadResourceContentAsync(string embeddedResourceName)
        {
            var assembly = Assembly.GetCallingAssembly();
            return await ReadResourceContentAsync(embeddedResourceName, assembly);
        }

        public static string ReadResourceContent(string embeddedResourceName, Assembly assembly)
        {
            string json;
            using (Stream stream = assembly.GetManifestResourceStream(embeddedResourceName))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException($"Embedded resource {embeddedResourceName} doesn't exist.");
                }
                using (TextReader reader = new StreamReader(stream))
                {
                    json = reader.ReadToEnd();
                }
            }

            return json;
        }

        public static async Task<string> ReadResourceContentAsync(string embeddedResourceName, Assembly assembly)
        {
            string json;
            using (Stream stream = assembly.GetManifestResourceStream(embeddedResourceName))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException($"Embedded resource {embeddedResourceName} doesn't exist.");
                }
                using (TextReader reader = new StreamReader(stream))
                {
                    json = await reader.ReadToEndAsync();
                }
            }

            return json;
        }
    }
}