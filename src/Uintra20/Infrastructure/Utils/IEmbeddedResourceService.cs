using System.Reflection;
using System.Threading.Tasks;

namespace Uintra20.Infrastructure.Utils
{
    public interface IEmbeddedResourceService
    {
        string ReadResourceContent(string embeddedResourceName);
        string ReadResourceContent(string embeddedResourceName, Assembly assembly);

        Task<string> ReadResourceContentAsync(string embeddedResourceName);
        Task<string> ReadResourceContentAsync(string embeddedResourceName, Assembly assembly);
    }
}
