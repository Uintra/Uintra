using System;
using System.Threading.Tasks;
using System.Web;
using LightInject;
using LightInject.Web;
using Umbraco.Core.Composing;
using Umbraco.Core.Composing.LightInject;

namespace Uintra20.Infrastructure.Extensions
{
    public static class FactoryExtensions
    {
        public static async Task EnsureScopeAsync(this IFactory factory, Func<IServiceContainer, Task> func)
        {
            HttpContext.Current = HttpContext.Current ?? new HttpContext(new HttpRequest("", "http://localhost/", ""), new HttpResponse(null));

            var serviceContainer = factory.Concrete as IServiceContainer;
            var scopeManager = serviceContainer.ScopeManagerProvider.GetScopeManager(serviceContainer);
            if (scopeManager.CurrentScope == null)
            {
                serviceContainer.ScopeManagerProvider = new PerWebRequestScopeManagerProvider();
                using (serviceContainer.BeginScope())
                {
                    await func(serviceContainer);
                }
                serviceContainer.ScopeManagerProvider = new MixedLightInjectScopeManagerProvider();
            }
            else
            {
                await func(serviceContainer);
            }
        }
    }
}