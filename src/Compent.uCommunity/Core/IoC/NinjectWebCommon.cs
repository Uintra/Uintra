using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Compent.uCommunity.Core.CentralFeed;
using Compent.uCommunity.Core.Comments;
using Compent.uCommunity.Core.Events;
using Compent.uCommunity.Core.Exceptions;
using Compent.uCommunity.Core.Helpers;
using Compent.uCommunity.Core.IoC;
using Compent.uCommunity.Core.Navigation;
using Compent.uCommunity.Core.News;
using Compent.uCommunity.Core.Notification;
using Compent.uCommunity.Core.Subscribe;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Newtonsoft.Json.Serialization;
using Ninject;
using Ninject.Web.Common;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using uCommunity.CentralFeed;
using uCommunity.CentralFeed.Core;
using uCommunity.Comments;
using uCommunity.Core.Activity;
using uCommunity.Core.Activity.Sql;
using uCommunity.Core.Caching;
using uCommunity.Core.Configuration;
using uCommunity.Core.Exceptions;
using uCommunity.Core.Grid;
using uCommunity.Core.Localization;
using uCommunity.Core.Media;
using uCommunity.Core.ModelBinders;
using uCommunity.Core.Persistence.Sql;
using uCommunity.Core.User;
using uCommunity.Core.User.Permissions;
using uCommunity.Events;
using uCommunity.Likes;
using uCommunity.Navigation.Core;
using uCommunity.Navigation.Core.Dashboard;
using uCommunity.News;
using uCommunity.Notification;
using uCommunity.Notification.Core.Configuration;
using uCommunity.Notification.Core.Services;
using uCommunity.Notification.Core.Sql;
using uCommunity.Subscribe;
using uCommunity.Users.Core;
using Umbraco.Core;
using Umbraco.Core.Configuration;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Routing;
using Umbraco.Web.Security;
using SqlNotification = uCommunity.Notification.Core.Sql.Notification;
using SqlSubscribe = uCommunity.Subscribe.Subscribe;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(NinjectWebCommon), "PostStart")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(NinjectWebCommon), "Stop")]

namespace Compent.uCommunity.Core.IoC
{
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

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

            var notificationConfigurationProvider = kernel.Get<IConfigurationProvider<NotificationConfiguration>>();
            notificationConfigurationProvider.Initialize();

            var reminderConfigurationProvider = kernel.Get<IConfigurationProvider<ReminderConfiguration>>();
            reminderConfigurationProvider.Initialize();
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
                RegisterModelBinders();
                RegisterGlobalFilters(kernel);

                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        private static void RegisterModelBinders()
        {
            ModelBinders.Binders.DefaultBinder = new CustomModelBinder();
        }

        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IPermissionsConfiguration>().ToMethod(s => PermissionsConfiguration.Configure).InSingletonScope();
            kernel.Bind<IPermissionsService>().To<PermissionsService>().InRequestScope();

            // Umbraco
            kernel.Bind<UmbracoContext>().ToMethod(context => CreateUmbracoContext()).InRequestScope();
            kernel.Bind<UmbracoHelper>().ToSelf().InRequestScope();
            kernel.Bind<IUserService>().ToMethod(i => ApplicationContext.Current.Services.UserService).InRequestScope();
            kernel.Bind<IContentService>().ToMethod(i => ApplicationContext.Current.Services.ContentService).InRequestScope();
            kernel.Bind<IContentTypeService>().ToMethod(i => ApplicationContext.Current.Services.ContentTypeService).InRequestScope();
            kernel.Bind<IMediaService>().ToMethod(i => ApplicationContext.Current.Services.MediaService).InRequestScope();
            kernel.Bind<DatabaseContext>().ToMethod(i => ApplicationContext.Current.DatabaseContext).InRequestScope();
            kernel.Bind<IDataTypeService>().ToMethod(i => ApplicationContext.Current.Services.DataTypeService).InRequestScope();
            kernel.Bind<IMemberService>().ToMethod(i => ApplicationContext.Current.Services.MemberService).InRequestScope();

            // Plugin services
            kernel.Bind<IIntranetLocalizationService>().To<LocalizationService>().InRequestScope();
            kernel.Bind<IIntranetUserService>().To<IntranetUserService>().InRequestScope();
            kernel.Bind(typeof(INewsService)).To<NewsService>().InRequestScope();
            kernel.Bind(typeof(IEventsService)).To<EventsService>().InRequestScope();
            kernel.Bind<IMediaHelper>().To<MediaHelper>().InRequestScope();
            kernel.Bind<IIntranetActivityRepository>().To<IntranetActivityRepository>().InRequestScope();
            kernel.Bind<ICacheService>().To<MemoryCacheService>().InRequestScope();

            kernel.Bind<IDbConnectionFactory>().ToMethod(i => new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ConnectionString, SqlServerDialect.Provider)).InSingletonScope();
            kernel.Bind<ICommentsService>().To<CommentsService>().InRequestScope();
            kernel.Bind<ICommentsPageHelper>().To<CommentsPageHelper>().InRequestScope();
            kernel.Bind<ICommentableService>().To<CustomCommentableService>().InRequestScope();

            kernel.Bind<ILikesService>().To<LikesService>().InRequestScope();

            kernel.Bind<ICentralFeedService>().To<CentralFeedService>().InRequestScope();
            kernel.Bind<ICentralFeedItem>().To<News.Entities.NewsEntity>().InRequestScope();
            kernel.Bind<ICentralFeedContentHelper>().To<CentralFeedContentHelper>().InRequestScope();
            kernel.Bind<ICentralFeedItemService>().To<NewsService>().InRequestScope();
            kernel.Bind<ICentralFeedItemService>().To<EventsService>().InRequestScope();

            kernel.Bind<ISubscribeService>().To<CustomSubscribeService>().InRequestScope();

            kernel.Bind<IUmbracoContentHelper>().To<UmbracoContentHelper>().InRequestScope();

            // Navigation 
            kernel.Bind<IConfigurationProvider<NavigationConfiguration>>().To<ConfigurationProvider<NavigationConfiguration>>().InSingletonScope()
                .WithConstructorArgument("settingsFilePath", "~/App_Plugins/Navigation/config/navigationConfiguration.json");

            kernel.Bind<INavigationCompositionService>().To<NavigationCompositionService>().InRequestScope();
            kernel.Bind<IHomeNavigationCompositionService>().To<HomeNavigationCompositionService>().InRequestScope();
            kernel.Bind<ILeftSideNavigationModelBuilder>().To<UcommunityLeftSideNavigationModelBuilder>().InRequestScope();
            kernel.Bind<ISubNavigationModelBuilder>().To<SubNavigationModelBuilder>().InRequestScope();
            kernel.Bind<ITopNavigationModelBuilder>().To<TopNavigationModelBuilder>().InRequestScope();
            kernel.Bind<IMyLinksModelBuilder>().To<MyLinksModelBuilder>().InRequestScope();
            kernel.Bind<IMyLinksService>().To<MyLinksService>().InRequestScope();

            // Notifications
            kernel.Bind<IConfigurationProvider<NotificationConfiguration>>().To<NotificationConfigurationProvider>().InSingletonScope()
                .WithConstructorArgument(typeof(string), "~/App_Plugins/Notification/config/notificationConfiguration.json");
            kernel.Bind<IConfigurationProvider<ReminderConfiguration>>().To<ConfigurationProvider<ReminderConfiguration>>().InSingletonScope()
                .WithConstructorArgument(typeof(string), "~/App_Plugins/Notification/config/reminderConfiguration.json");
            kernel.Bind<INotificationHelper>().To<NotificationHelper>().InRequestScope();
            kernel.Bind<INotifierService>().To<UiNotifierService>().InRequestScope();
            kernel.Bind<IUiNotifierService>().To<UiNotifierService>().InRequestScope();
            kernel.Bind<INotificationsService>().To<NotificationsService>().InRequestScope();
            kernel.Bind<IReminderService>().To<ReminderService>().InRequestScope();
            kernel.Bind<IReminderJob>().To<ReminderJob>().InRequestScope();

            // Factories
            kernel.Bind<IActivitiesServiceFactory>().To<ActivitiesServiceFactory>().InRequestScope();

            kernel.Bind<IExceptionLogger>().To<ExceptionLogger>().InRequestScope();

            // Model Binders
            kernel.Bind<DateTimeBinder>().ToSelf().InSingletonScope();

            //Sql 
            kernel.Bind(typeof(ISqlRepository<>)).To(typeof(SqlRepository<>)).InRequestScope();
            EnsureTablesExists(kernel);

            kernel.Bind<IGridHelper>().To<GridHelper>().InRequestScope();

        }

        private static void RegisterGlobalFilters(IKernel kernel)
        {
            GlobalFilters.Filters.Add(new System.Web.Mvc.AuthorizeAttribute());
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

        private static void EnsureTablesExists(IKernel kernel)
        {
            var sqlTypes = Assembly.GetAssembly(typeof(SqlEntity))
                            .GetTypes()
                            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(SqlEntity))).ToList();
            sqlTypes.Add(typeof(Comment));
            sqlTypes.Add(typeof(Like));
            sqlTypes.Add(typeof(SqlSubscribe));
            sqlTypes.Add(typeof(IntranetActivityEntity));
            sqlTypes.Add(typeof(SqlNotification));
            sqlTypes.Add(typeof(Reminder));
            sqlTypes.Add(typeof(MyLink));

            var connectionFactory = (IDbConnectionFactory)kernel.GetService(typeof(IDbConnectionFactory));
            using (var conn = connectionFactory.Open())
            {
                foreach (var sqlType in sqlTypes)
                {
                    conn.CreateTableIfNotExists(sqlType);
                }
            }
        }
    }
}
