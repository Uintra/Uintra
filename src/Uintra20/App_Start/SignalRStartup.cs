using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LightInject;
using LightInject.Mvc;
using LightInject.Web;
using Microsoft.AspNet.SignalR;
using Owin;
using Umbraco.Core.Composing;
using Umbraco.Core.Composing.LightInject;
using Umbraco.Core.Configuration;

namespace Uintra20
{
	public static class SignalRStartup
	{
		public static void ConfigureSignalR(this IAppBuilder appBuilder, IGlobalSettings globalSettings)
		{
			var dependencyResolver = DependencyResolver.Current as LightInjectMvcDependencyResolver;
			var serviceContainer = typeof(LightInjectMvcDependencyResolver)
				.GetField("serviceContainer", BindingFlags.Instance | BindingFlags.NonPublic)
				.GetValue(dependencyResolver) as ServiceContainer;

			ConfigureSignalRPipeline(appBuilder, serviceContainer,globalSettings);
		}

		private static void ConfigureSignalRPipeline(IAppBuilder appBuilder, IServiceContainer serviceContainer, IGlobalSettings globalSettings)
		{
			var config = serviceContainer.EnableSignalR();

			serviceContainer.Register<LightInjectScopedConnection>();
			serviceContainer.RegisterInstance<IServiceContainer>(serviceContainer);
			serviceContainer.RegisterInstance<HubConfiguration>(config);
			GlobalHost.DependencyResolver = config.Resolver;

			var umbracoPath = globalSettings.GetUmbracoMvcArea();
			var signalrPath = HttpRuntime.AppDomainAppVirtualPath + umbracoPath + "/BackOffice/signalr";
			appBuilder.MapSignalR<LightInjectScopedConnection>(signalrPath, new HubConfiguration { EnableDetailedErrors = true });
			serviceContainer.EnablePerWebRequestScope();
		}

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