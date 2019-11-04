using System.Reflection;
using System.Threading.Tasks;

namespace Uintra20.Core.Utils
{
    public class EmbeddedResourceService : IEmbeddedResourceService
    {
        public string ReadResourceContent(string embeddedResourceName) =>
            EmbeddedResourcesUtils.ReadResourceContent(embeddedResourceName);

        public string ReadResourceContent(string embeddedResourceName, Assembly assembly) =>
            EmbeddedResourcesUtils.ReadResourceContent(embeddedResourceName, assembly);

        public async Task<string> ReadResourceContentAsync(string embeddedResourceName) =>
            await EmbeddedResourcesUtils.ReadResourceContentAsync(embeddedResourceName);

        public async Task<string> ReadResourceContentAsync(string embeddedResourceName, Assembly assembly) =>
            await EmbeddedResourcesUtils.ReadResourceContentAsync(embeddedResourceName, assembly);
    }
}