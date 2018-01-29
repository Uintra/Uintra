using System.IO;
using System.Xml.Serialization;

namespace uIntra.Core.Utils
{
    public static class FileSystemUtils
    {
        public static T ReadXmlFile<T>(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}
