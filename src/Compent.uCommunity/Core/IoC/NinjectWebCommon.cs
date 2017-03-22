using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Compent.uCommunity.Core.IoC;
using Compent.uCommunity.Core.Users;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Newtonsoft.Json.Serialization;
using Ninject;
using Ninject.Web.Common;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using uCommunity.Comments;
using uCommunity.Core.Activity;
using uCommunity.Core.Activity.Sql;
using uCommunity.Core.Caching;
using uCommunity.Core.Configuration;
using uCommunity.Core.Localization;
using uCommunity.Core.Media;
using uCommunity.Core.Persistence.Sql;
using uCommunity.Core.User;
using uCommunity.Likes;
using uCommunity.Navigation.Core;
using uCommunity.Navigation.Core.Dashboard;
using uCommunity.News;
using Umbraco.Core;
using Umbraco.Core.Configuration;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Routing;
using Umbraco.Web.Security;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(NinjectWebCommon), "PostStart")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(NinjectWebCommon), "Stop")]

namespace Compent.uCommunity.Core.IoC
{
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        private static readonly string TDIntranetConnectionString = @"server=192.168.0.208\SQL2014;database=TD_Intranet_controls;user id=sa;password='q1w2e3r4'";

        public static void Start()

        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            GlobalConfiguration.Configure((config) =>
            {
                config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
        }

        public static void PostStart()
        {
            var kernel = bootstrapper.Kernel;

            var configurationProvider = kernel.Get<IConfigurationProvider<NavigationConfiguration>>();
            configurationProvider.Initialize();
        }

        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);

                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        private static void RegisterServices(IKernel kernel)
        {
            // Umbraco
            kernel.Bind<UmbracoContext>().ToMethod(context => CreateUmbracoContext()).InRequestScope();
            kernel.Bind<UmbracoHelper>().ToSelf().InRequestScope();
            kernel.Bind<IUserService>().ToMethod(i => ApplicationContext.Current.Services.UserService).InRequestScope();
            kernel.Bind<IContentService>().ToMethod(i => ApplicationContext.Current.Services.ContentService).InRequestScope();
            kernel.Bind<IContentTypeService>().ToMethod(i => ApplicationContext.Current.Services.ContentTypeService).InRequestScope();
            kernel.Bind<IMediaService>().ToMethod(i => ApplicationContext.Current.Services.MediaService).InRequestScope();
            kernel.Bind<DatabaseContext>().ToMethod(i => ApplicationContext.Current.DatabaseContext).InRequestScope();
            kernel.Bind<IDataTypeService>().ToMethod(i => ApplicationContext.Current.Services.DataTypeService).InRequestScope();

            // Plugin services
            kernel.Bind<IIntranetLocalizationService>().To<LocalizationService>().InRequestScope();
            kernel.Bind(typeof(IIntranetUserService<>)).To<IntranetUserService>().InRequestScope();
            kernel.Bind(typeof(INewsService<,>)).To<NewsService>().InRequestScope();
            kernel.Bind<IMediaHelper>().To<MediaHelper>().InRequestScope();
            kernel.Bind<IIntranetActivityService>().To<IntranetActivityService>().InRequestScope();
            kernel.Bind<IMemoryCacheService>().To<MemoryCacheService>().InRequestScope();

            kernel.Bind<IDbConnectionFactory>().ToMethod(i => new OrmLiteConnectionFactory(TDIntranetConnectionString, SqlServerDialect.Provider)).InSingletonScope();

            kernel.Bind<ISqlRepository<Comment>>().To<SqlRepository<Comment>>().InRequestScope();
            kernel.Bind<ISqlRepository<Like>>().To<SqlRepository<Like>>().InRequestScope();
            kernel.Bind<ISqlRepository<IntranetActivityEntity>>().To<SqlRepository<IntranetActivityEntity>>().InRequestScope();
            kernel.Bind<ICommentsService>().To<CommentsService>().InRequestScope();

            kernel.Bind<ILikesService>().To<LikesService>().InRequestScope();
            kernel.Bind<ILikeableService>().To<LikeableService>().InRequestScope();
            kernel.Bind<IIntranetUserService>().To<IntranetUserService>().InRequestScope();

            // Navigation 
            kernel.Bind<IConfigurationProvider<NavigationConfiguration>>().To<ConfigurationProvider<NavigationConfiguration>>().InSingletonScope()
                .WithConstructorArgument("settingsFilePath", "~/App_Plugins/Navigation/config/navigationConfiguration.json");

            kernel.Bind<INavigationCompositionService>().To<NavigationCompositionService>().InRequestScope();
            kernel.Bind<IHomeNavigationCompositionService>().To<HomeNavigationCompositionService>().InRequestScope();
            kernel.Bind<ILeftSideNavigationModelBuilder>().To<LeftSideNavigationModelBuilder>().InRequestScope();
            kernel.Bind<ISubNavigationModelBuilder>().To<SubNavigationModelBuilder>().InRequestScope();
            kernel.Bind<ITopNavigationModelBuilder>().To<TopNavigationModelBuilder>().InRequestScope();
        }

        private static UmbracoContext CreateUmbracoContext()
        {
            var context = HttpContext.Current ?? new HttpContext(new HttpRequest("", "http://localhost/", ""), new HttpResponse(null));
            var httpContextWrapper = new HttpContextWrapper(context);
            var umbracoSettings = UmbracoConfig.For.UmbracoSettings();
            var urlProvider = UrlProviderResolver.Current.Providers;
            var webSecurity = new WebSecurity(httpContextWrapper, ApplicationContext.Current);
            var result = UmbracoContext.EnsureContext(httpContextWrapper, ApplicationContext.Current, webSecurity, umbracoSettings, urlProvider, false);
            return result;
        }
    }
}
