using Compent.Shared.ConfigurationProvider.Json;
using Compent.Shared.DependencyInjection.Contract;
using Compent.Shared.DependencyInjection.LightInject;
using Compent.Shared.Logging.Serilog;
using LightInject;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Extensions.Configuration;
using System.Web;
using Compent.Shared.Search.Elasticsearch;
using UBaseline.Core.Startup;
using Uintra.Models.UmbracoIdentity;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Web;
using UmbracoIdentity;
using Uintra.Core.Updater;

namespace Uintra
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Boot)]
    public class StartupComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Register(x =>
            {
                //needs to resolve from Owin
                var owinCtx = x.GetInstance<IHttpContextAccessor>().HttpContext.GetOwinContext();
                return owinCtx.GetUserManager<UmbracoMembersUserManager<UmbracoApplicationMember>>();
            }, Lifetime.Request);

            composition.Register(x =>
            {
                //needs to resolve from Owin
                var owinCtx = x.GetInstance<IHttpContextAccessor>().HttpContext.GetOwinContext();
                return owinCtx.GetUserManager<UmbracoMembersRoleManager<UmbracoApplicationRole>>();
            }, Lifetime.Request);

            var container = composition.Concrete as IServiceContainer;

            var builder = new JsonConfigurationBuilder(new ConfigurationBuilder());
            var configuration = builder
                .AddLogging(UBaselineConfiguration.EnvironmentName)
                .AddUBaselineConfiguration()
                .AddConfiguration()
                .AddUintraConfiguration()
                .Build();

			var assembly = typeof(MigrationExecutor).Assembly;

			var dependencyCollection = new LightInjectDependencyCollection(container, configuration);
			dependencyCollection
                .AddLogging()
                .AddUBaseline()
                .RegisterInjectModules(assembly)
                .RegisterMvcControllers(assembly)
                .RegisterApiControllers(assembly)
                .RegisterConverters(assembly);

			//composition.Components().Append<UintraApplicationComponent>();
            //composition.Components().Append<UintraUmbracoEventComponent>();

            MapperConfig.RegisterMappings(composition);
        }
    }
}