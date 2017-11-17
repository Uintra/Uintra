using System.Reflection;

namespace uIntra.Core.Utils
{
    public interface IEmbeddedResourceService
    {
        string ReadResourceContent(string embeddedResourceName);
        string ReadResourceContent(string embeddedResourceName, Assembly assembly);
    }
}