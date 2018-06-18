using System.IO;
using System.Reflection;

namespace uIntra.Core.Utils
{
    public static class EmbeddedResourcesUtils
    {       
        public static string ReadResourceContent(string embeddedResourceName)
        {
            var assembly = Assembly.GetCallingAssembly();
            return ReadResourceContent(embeddedResourceName, assembly);
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
    }
}