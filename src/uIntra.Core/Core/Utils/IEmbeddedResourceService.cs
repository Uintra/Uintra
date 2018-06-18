using System.Reflection;

namespace Uintra.Core.Utils
{
    public interface IEmbeddedResourceService
    {
        string ReadResourceContent(string embeddedResourceName);
        string ReadResourceContent(string embeddedResourceName, Assembly assembly);
    }
}