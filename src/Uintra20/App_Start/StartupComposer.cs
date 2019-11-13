using LightInject;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace Uintra20.App_Start
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Boot)]
    public class StartupComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            var container = composition.Concrete as IServiceContainer;

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

            LightInjectWebCommon.Start(composition);
            MapperConfig.RegisterMappings();
        }
    }


}