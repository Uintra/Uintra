using LightInject;
using Uintra20.Infrastructure.Ioc;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Composing.LightInject;

namespace Uintra20.App_Start
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Boot)]
    public class StartupComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            var container = composition.Concrete as ServiceContainer;

            var container1 = new LightInjectContainerProvider(container);

            //var builder = new JsonConfigurationBuilder(new ConfigurationBuilder());
            //var configuration = builder
            //    .AddLogging(UBaselineConfiguration.EnvironmentName)
            //    .AddUBaselineConfiguration()
            //    .Build();

            //var assembly = typeof(StartupComposer).Assembly;

            //var dependencyCollection = new LightInjectDependencyCollection(container, configuration);
            //dependencyCollection.AddLogging()
            //    .AddUBaseline()
            //    .RegisterInjectModules(assembly)
            //    .RegisterMvcControllers(assembly)
            //    .RegisterApiControllers(assembly)
            //    .RegisterConverters(assembly);

            LightInjectWebCommon.Start(container1);

            MapperConfig.RegisterMappings();
        }
    }


}