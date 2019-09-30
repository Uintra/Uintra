using System.Reflection;
using System.Web;

namespace Uintra.Core.UmbracoIpAccess
{
    public interface IUmbracoIpAccessValidator
    {
        void Validate(HttpContext httpContext, Assembly controllerAssembly);
    }
}