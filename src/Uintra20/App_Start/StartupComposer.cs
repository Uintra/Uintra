using Compent.Shared.ConfigurationProvider.Json;
using Compent.Shared.DependencyInjection.Contract;
using Compent.Shared.DependencyInjection.LightInject;
using Compent.Shared.Logging.Serilog;
using LightInject;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Extensions.Configuration;
using System.Web;
using System.Web.Mvc;
using UBaseline.Core.Startup;
using Uintra20.Core.Configuration;
using Uintra20.Features.Navigation.Configuration;
using Uintra20.Models.UmbracoIdentity;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Web;
using UmbracoIdentity;

namespace Uintra20
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
                .Build();

			var assembly = typeof(StartupComposer).Assembly;

			var dependencyCollection = new LightInjectDependencyCollection(container, configuration);
			dependencyCollection.AddLogging()
                .AddLogging()
                .AddUBaseline()
                .RegisterInjectModules(assembly)
                .RegisterMvcControllers(assembly)
                .RegisterApiControllers(assembly)
                .RegisterConverters(assembly);

            MapperConfig.RegisterMappings(composition);
        }
    }


}