using System;
using System.Web;
using LightInject;
using LightInject.Web;
using UBaseline.Core.Infrastructure.Extensions;
using Umbraco.Core.Composing;
using Umbraco.Core.Composing.LightInject;

namespace Uintra20.Infrastructure.Extensions
{
	public static class FactoryExtensions
	{
		public static T EnsureScope<T>(this IFactory factory, Func<IServiceContainer, T> action)
		{
			T result = default;
			var serviceContainer = factory.Concrete as IServiceContainer;
			if (HttpContext.Current == null)
				HttpContext.Current.InitDefault();
			var scopeManager = serviceContainer.ScopeManagerProvider.GetScopeManager(serviceContainer);
			if (scopeManager.CurrentScope == null)
			{
				try
				{
					serviceContainer.ScopeManagerProvider = new PerWebRequestScopeManagerProvider();
					using (serviceContainer.BeginScope())
					{
						result = action(serviceContainer);
					}
				}
				catch (Exception ex)
				{
					Current.Logger.Error(typeof(FactoryExtensions), ex);
				}
				finally
				{
					serviceContainer.ScopeManagerProvider = new MixedLightInjectScopeManagerProvider();
					HttpContext.Current = null;
				}
			}
			else
			{
				result = action(serviceContainer);
			}
			return result;
		}
	}
}