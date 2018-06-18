using System.Reflection;

namespace Uintra.Core.Utils
{
    public class EmbeddedResourceService : IEmbeddedResourceService
    {
        public string ReadResourceContent(string embeddedResourceName) => 
            EmbeddedResourcesUtils.ReadResourceContent(embeddedResourceName);

        public string ReadResourceContent(string embeddedResourceName, Assembly assembly) => 
            EmbeddedResourcesUtils.ReadResourceContent(embeddedResourceName, assembly);
    }
}