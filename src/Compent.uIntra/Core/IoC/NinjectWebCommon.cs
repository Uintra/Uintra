using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Compent.uIntra.Core.ApplicationSettings;
using Compent.uIntra.Core.Bulletins;
using Compent.uIntra.Core.CentralFeed;
using Compent.uIntra.Core.Comments;
using Compent.uIntra.Core.Events;
using Compent.uIntra.Core.Exceptions;
using Compent.uIntra.Core.Helpers;
using Compent.uIntra.Core.IoC;
using Compent.uIntra.Core.Navigation;
using Compent.uIntra.Core.News;
using Compent.uIntra.Core.Notification;
using Compent.uIntra.Core.Subscribe;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Newtonsoft.Json.Serialization;
using Ninject;
using Ninject.Web.Common;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using uIntra.Bulletins;
using uIntra.CentralFeed;
using uIntra.Comments;
using uIntra.Core;
using uIntra.Core.Activity;
using uIntra.Core.ApplicationSettings;
using uIntra.Core.Caching;
using uIntra.Core.Configuration;
using uIntra.Core.Exceptions;
using uIntra.Core.Grid;
using uIntra.Core.Localization;
using uIntra.Core.Media;
using uIntra.Core.ModelBinders;
using uIntra.Core.Persistence;
using uIntra.Core.User;
using uIntra.Core.User.Permissions;
using uIntra.Events;
using uIntra.Likes;
using uIntra.Navigation;
using uIntra.Navigation.Configuration;
using uIntra.Navigation.Dashboard;
using uIntra.Navigation.MyLinks;
using uIntra.Navigation.SystemLinks;
using uIntra.News;
using uIntra.Notification;
using uIntra.Notification.Configuration;
using uIntra.Subscribe;
using uIntra.Tagging;
using uIntra.Users;
using Umbraco.Core;
using Umbraco.Core.Configuration;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Routing;
using Umbraco.Web.Security;
using SqlNotification = uIntra.Notification.Notification;
using SqlSubscribe = uIntra.Subscribe.Subscribe;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(NinjectWebCommon), "PostStart")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(NinjectWebCommon), "Stop")]

namespace Compent.uIntra.Core.IoC
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
            kernel.Bind(typeof(IIntranetUserService<>)).To<IntranetUserService>().InRequestScope();
            kernel.Bind(typeof(INewsService<>)).To<NewsService>().InRequestScope();
            kernel.Bind(typeof(IEventsService<>)).To<EventsService>().InRequestScope();
            kernel.Bind(typeof(IBulletinsService<>)).To<BulletinsService>().InRequestScope();
            kernel.Bind<IMediaHelper>().To<MediaHelper>().InRequestScope();
            kernel.Bind<IIntranetActivityRepository>().To<IntranetActivityRepository>().InRequestScope();
            kernel.Bind<ICacheService>().To<MemoryCacheService>().InRequestScope();
            kernel.Bind<IRoleService>().To<RoleServiceBase>().InRequestScope();

            kernel.Bind<IDbConnectionFactory>().ToMethod(i => new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ConnectionString, SqlServerDialect.Provider)).InSingletonScope();
            kernel.Bind<ICommentsService>().To<CommentsService>().InRequestScope();
            kernel.Bind<ICommentsPageHelper>().To<CommentsPageHelper>().InRequestScope();
            kernel.Bind<ICommentableService>().To<CustomCommentableService>().InRequestScope();

            kernel.Bind<ILikesService>().To<LikesService>().InRequestScope();

            kernel.Bind<ICentralFeedService>().To<CentralFeedService>().InRequestScope();
            kernel.Bind<ICentralFeedItem>().To<News.Entities.News>().InRequestScope();
            kernel.Bind<ICentralFeedContentHelper>().To<CentralFeedContentHelper>().InRequestScope();
            kernel.Bind<ICentralFeedItemService>().To<NewsService>().InRequestScope();
            kernel.Bind<ICentralFeedItemService>().To<EventsService>().InRequestScope();
            kernel.Bind<ICentralFeedItemService>().To<BulletinsService>().InRequestScope();

            kernel.Bind<ISubscribeService>().To<CustomSubscribeService>().InRequestScope();
            kernel.Bind<ITagsService>().To<TagsService>().InRequestScope();

            kernel.Bind<IUmbracoContentHelper>().To<UmbracoContentHelper>().InRequestScope();

            // Navigation 
            kernel.Bind<IConfigurationProvider<NavigationConfiguration>>().To<ConfigurationProvider<NavigationConfiguration>>().InSingletonScope()
                .WithConstructorArgument("settingsFilePath", "~/App_Plugins/Navigation/config/navigationConfiguration.json");

            kernel.Bind<INavigationCompositionService>().To<NavigationCompositionService>().InRequestScope();
            kernel.Bind<IHomeNavigationCompositionService>().To<HomeNavigationCompositionService>().InRequestScope();
            kernel.Bind<ILeftSideNavigationModelBuilder>().To<uIntraLeftSideNavigationModelBuilder>().InRequestScope();
            kernel.Bind<ISubNavigationModelBuilder>().To<SubNavigationModelBuilder>().InRequestScope();
            kernel.Bind<ITopNavigationModelBuilder>().To<TopNavigationModelBuilder>().InRequestScope();
            kernel.Bind<IMyLinksModelBuilder>().To<MyLinksModelBuilder>().InRequestScope();
            kernel.Bind<IMyLinksService>().To<MyLinksService>().InRequestScope();
            kernel.Bind<ISystemLinksModelBuilder>().To<SystemLinksModelBuilder>().InRequestScope();
            kernel.Bind<ISystemLinksService>().To<SystemLinksService>().InRequestScope();

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

            kernel.Bind<IApplicationSettings>().To<uIntraApplicationSettings>().InSingletonScope();
            kernel.Bind<IuIntraApplicationSettings>().To<uIntraApplicationSettings>().InSingletonScope();

            kernel.Bind<IDateTimeFormatProvider>().To<DateTimeFormatProvider>().InSingletonScope();
            kernel.Bind<ITimezoneOffsetProvider>().To<TimezoneOffsetProvider>().InSingletonScope();
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
            sqlTypes.Add(typeof(Tag));
            sqlTypes.Add(typeof(TagActivityRelation));

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
