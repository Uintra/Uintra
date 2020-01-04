using System.Reflection;
using System.Web.Mvc;
using LightInject;
using LightInject.Mvc;
using Microsoft.AspNet.SignalR;
using Owin;
using Uintra20.Infrastructure.Ioc;

namespace Uintra20.Core.Hubs
{
	public static class SignalRStartup
	{
		public static void ConfigureSignalR(this IAppBuilder appBuilder)
		{
			var dependencyResolver = DependencyResolver.Current as LightInjectMvcDependencyResolver;
			var serviceContainer = typeof(LightInjectMvcDependencyResolver)
				.GetField("serviceContainer", BindingFlags.Instance | BindingFlags.NonPublic)
				.GetValue(dependencyResolver) as ServiceContainer;

			ConfigureSignalRPipeline(appBuilder, serviceContainer);
		}

		private static void ConfigureSignalRPipeline(IAppBuilder appBuilder, IServiceContainer serviceContainer)
		{
			var config = serviceContainer.EnableSignalR();

			serviceContainer.Register<LightInjectScopedConnection>();
			serviceContainer.RegisterInstance<IServiceContainer>(serviceContainer);
			serviceContainer.RegisterInstance<HubConfiguration>(config);
			GlobalHost.DependencyResolver = config.Resolver;

			appBuilder.MapSignalR<LightInjectScopedConnection>("/signalr", config);
			serviceContainer.EnablePerWebRequestScope();
		}

		
	}
}