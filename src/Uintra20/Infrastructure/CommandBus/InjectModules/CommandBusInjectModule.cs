using Compent.CommandBus;
using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Infrastructure.CommandBus.Configurations;
using Uintra20.Infrastructure.CommandBus.Resolvers;

namespace Uintra20.Infrastructure.CommandBus.InjectModules
{
    public class CommandBusInjectModule : IInjectModule
	{
		public IDependencyCollection Register(IDependencyCollection services)
		{
            services.AddScoped<IInstanceFactory, ReflectionInstanceFactory>();
            services.AddScoped<IDependencyResolver, CommandBusDependencyResolver>();
            services.AddScoped<ICommandPublisher, CommandPublisher>();
            services.AddScoped<IBusResolver, BusResolver>();
            services.AddSingleton<CommandBindingProviderBase, CommandBusConfiguration>();

            return services;
		}
	}
}