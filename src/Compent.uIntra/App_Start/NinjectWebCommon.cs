using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Compent.uIntra;
using Compent.uIntra.Core;
using Compent.uIntra.Core.Activity;
using Compent.uIntra.Core.ApplicationSettings;
using Compent.uIntra.Core.Bulletins;
using Compent.uIntra.Core.CentralFeed;
using Compent.uIntra.Core.Comments;
using Compent.uIntra.Core.Controls.EditorConfiguration;
using Compent.uIntra.Core.Events;
using Compent.uIntra.Core.Exceptions;
using Compent.uIntra.Core.Feed.Links;
using Compent.uIntra.Core.Groups;
using Compent.uIntra.Core.Handlers;
using Compent.uIntra.Core.Helpers;
using Compent.uIntra.Core.IoC;
using Compent.uIntra.Core.Licence;
using Compent.uIntra.Core.News;
using Compent.uIntra.Core.Notification;
using Compent.uIntra.Core.PagePromotion;
using Compent.uIntra.Core.Search;
using Compent.uIntra.Core.Subscribe;
using Compent.uIntra.Core.Users;
using Compent.uIntra.Persistence.Sql;
using EmailWorker.Ninject;
using Localization.Core;
using Localization.Core.Configuration;
using Localization.Storage.UDictionary;
using Localization.Umbraco;
using Localization.Umbraco.Export;
using Localization.Umbraco.Import;
using Localization.Umbraco.UmbracoEvents;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Nest;
using Newtonsoft.Json.Serialization;
using Ninject;
using Ninject.Web.Common;
using uIntra.Bulletins;
using uIntra.CentralFeed;
using uIntra.CentralFeed.Providers;
using uIntra.Comments;
using uIntra.Core;
using uIntra.Core.Activity;
using uIntra.Core.ApplicationSettings;
using uIntra.Core.Attributes;
using uIntra.Core.BrowserCompatibility;
using uIntra.Core.Caching;
using uIntra.Core.Configuration;
using uIntra.Core.Controls;
using uIntra.Core.Exceptions;
using uIntra.Core.Grid;
using uIntra.Core.Links;
using uIntra.Core.Localization;
using uIntra.Core.Media;
using uIntra.Core.MigrationHistories;
using uIntra.Core.ModelBinders;
using uIntra.Core.PagePromotion;
using uIntra.Core.Persistence;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Core.User.Permissions;
using uIntra.Core.Utils;
using uIntra.Events;
using uIntra.Groups;
using uIntra.LicenceService.ApiClient;
using uIntra.LicenceService.ApiClient.Interfaces;
using uIntra.Likes;
using uIntra.Navigation;
using uIntra.Navigation.Configuration;
using uIntra.Navigation.Dashboard;
using uIntra.Navigation.EqualityComparers;
using uIntra.Navigation.MyLinks;
using uIntra.Navigation.SystemLinks;
using uIntra.News;
using uIntra.Notification;
using uIntra.Notification.Configuration;
using uIntra.Notification.DefaultImplementation;
using uIntra.Search;
using uIntra.Search.Configuration;
using uIntra.Subscribe;
using uIntra.Users;
using Umbraco.Core;
using Umbraco.Core.Configuration;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;
using Umbraco.Core.Services;
using uIntra.Core.UmbracoEventServices;
using Umbraco.Web;
using Umbraco.Web.Routing;
using Umbraco.Web.Security;
using MyLinksModelBuilder = Compent.uIntra.Core.Navigation.MyLinksModelBuilder;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using uIntra.Tagging.UserTags;
using Umbraco.Core.Publishing;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(NinjectWebCommon), "PostStart")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(NinjectWebCommon), "Stop")]

namespace Compent.uIntra
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

            UmbracoEventsModule.RegisterEvents();

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
            var kernel = new StandardKernel(new EmailWorkerNinjectModule());
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterEntityFrameworkServices(kernel);
                RegisterServices(kernel);
                RegisterModelBinders();
                RegisterGlobalFilters(kernel);
                RegisterLocalizationServices(kernel);
                RegisterSearchServices(kernel);

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
            kernel.Bind<IBrowserCompatibilityConfigurationSection>().ToMethod(s => BrowserCompatibilityConfigurationSection.Configuration).InSingletonScope();
            kernel.Bind<IPermissionsConfiguration>().ToMethod(s => PermissionsConfiguration.Configure).InSingletonScope();
            kernel.Bind<IPermissionsService>().To<PermissionsService>().InRequestScope();

            //licence
            kernel.Bind<ILicenceValidationServiceClient>().To<LicenceValidationServiceClient>().InRequestScope();
            kernel.Bind<IValidateLicenceService>().To<ValidateLicenceService>().InRequestScope();
            kernel.Bind<IWebApiClient>().ToMethod((ctx => new WebApiClient() { Connection = new LicenceServiceConnection() })).InSingletonScope();
            kernel.Bind<ILicenceRequestHandler>().To<LicenceRequestHandler>().InRequestScope();


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
            kernel.Bind<IMemberTypeService>().ToMethod(i => ApplicationContext.Current.Services.MemberTypeService).InRequestScope();
            kernel.Bind<IMemberGroupService>().ToMethod(i => ApplicationContext.Current.Services.MemberGroupService).InRequestScope();
            kernel.Bind<ILocalizationService>().ToMethod(i => ApplicationContext.Current.Services.LocalizationService).InRequestScope();
            kernel.Bind<IDomainService>().ToMethod(i => ApplicationContext.Current.Services.DomainService).InRequestScope();

            // Plugin services
            kernel.Bind<IIntranetLocalizationService>().To<Core.LocalizationService>().InRequestScope();
            kernel.Bind(typeof(IIntranetUserService<>)).To<IntranetUserService<IntranetUser>>().InRequestScope();
            kernel.Bind<ICacheableIntranetUserService>().To<IntranetUserService<IntranetUser>>().InRequestScope();
            kernel.Bind(typeof(INewsService<>)).To<NewsService>().InRequestScope();
            kernel.Bind(typeof(IEventsService<>)).To<EventsService>().InRequestScope();
            kernel.Bind(typeof(IBulletinsService<>)).To<BulletinsService>().InRequestScope();
            kernel.Bind(typeof(IPagePromotionService<>)).To<PagePromotionService>().InRequestScope();
            kernel.Bind<IMediaHelper>().To<MediaHelper>().InRequestScope();
            kernel.Bind<IIntranetActivityRepository>().To<IntranetActivityRepository>().InRequestScope();
            kernel.Bind<ICacheService>().To<MemoryCacheService>().InRequestScope();
            kernel.Bind<IRoleService>().To<RoleServiceBase>().InRequestScope();
            kernel.Bind<IMemberServiceHelper>().To<MemberServiceHelper>().InRequestScope();
            kernel.Bind<IIntranetMediaService>().To<IntranetMediaService>().InRequestScope();
            kernel.Bind<IEditorConfigProvider>().To<IntranetEditorConfigProvider>().InRequestScope();
            kernel.Bind<IEmbeddedResourceService>().To<EmbeddedResourceService>().InRequestScope();
            
            kernel.Bind<ICommentsService>().To<CommentsService>().InRequestScope();
            kernel.Bind<ICommentsPageHelper>().To<CommentsPageHelper>().InRequestScope();
            kernel.Bind<ICommentableService>().To<CustomCommentableService>().InRequestScope();
            kernel.Bind<ICommentLinkHelper>().To<CommentLinkHelper>().InRequestScope();

            kernel.Bind<ILikesService>().To<LikesService>().InRequestScope();

            // Feed
            kernel.Bind<IFeedItemService>().To<NewsService>().InRequestScope();
            kernel.Bind<IFeedItemService>().To<EventsService>().InRequestScope();
            kernel.Bind<IFeedItemService>().To<BulletinsService>().InRequestScope();
            kernel.Bind<IFeedItemService>().To<PagePromotionService>().InRequestScope();

            kernel.Bind<ICentralFeedService>().To<CentralFeedService>().InRequestScope();
            kernel.Bind<IGroupFeedService>().To<GroupFeedService>().InRequestScope();

            kernel.Bind<ICentralFeedLinkProvider>().To<CentralFeedLinkProvider>();
            kernel.Bind<IGroupFeedLinkProvider>().To<GroupFeedLinkProvider>();
            kernel.Bind<IActivityLinkService>().To<ActivityLinkService>();
            kernel.Bind<ICentralFeedLinkService>().To<ActivityLinkService>();
            kernel.Bind<IGroupFeedLinkService>().To<ActivityLinkService>();


            kernel.Bind<IFeedActivityHelper>().To<FeedActivityHelper>();
            kernel.Bind<IGroupActivityService>().To<GroupActivityService>();
            kernel.Bind<IActivityTypeHelper>().To<ActivityTypeHelper>();
            kernel.Bind<IActivityPageHelperFactory>().To<CacheActivityPageHelperFactory>().InRequestScope();

            kernel.Bind<ICentralFeedContentService>().To<CentralFeedContentService>().InRequestScope();
            kernel.Bind<IGroupFeedContentService>().To<GroupFeedContentService>().InRequestScope();

            kernel.Bind<ICentralFeedContentProvider>().To<CentralFeedContentProvider>().InRequestScope();

            kernel.Bind<ICentralFeedHelper>().To<CentralFeedHelper>().InRequestScope();
            kernel.Bind<IGroupHelper>().To<GroupHelper>().InRequestScope();
            kernel.Bind<IFeedFilterStateService>().To<CentralFeedFilterStateService>().InRequestScope();


            kernel.Bind(typeof(IIntranetActivityService<>)).To<NewsService>().InRequestScope();
            kernel.Bind(typeof(IIntranetActivityService<>)).To<EventsService>().InRequestScope();
            kernel.Bind(typeof(IIntranetActivityService<>)).To<BulletinsService>().InRequestScope();
            kernel.Bind(typeof(IIntranetActivityService<>)).To<PagePromotionService>().InRequestScope();
            
            kernel.Bind<IIntranetActivityService>().To<NewsService>().InRequestScope();
            kernel.Bind<IIntranetActivityService>().To<EventsService>().InRequestScope();
            kernel.Bind<IIntranetActivityService>().To<BulletinsService>().InRequestScope();
            kernel.Bind<IIntranetActivityService>().To<PagePromotionService>().InRequestScope();

            kernel.Bind<ISubscribableService>().To<EventsService>().InRequestScope();

            kernel.Bind<ILikeableService>().To<NewsService>().InRequestScope();
            kernel.Bind<ILikeableService>().To<EventsService>().InRequestScope();
            kernel.Bind<ILikeableService>().To<BulletinsService>().InRequestScope();

            kernel.Bind<ICommentableService>().To<NewsService>().InRequestScope();
            kernel.Bind<ICommentableService>().To<EventsService>().InRequestScope();
            kernel.Bind<ICommentableService>().To<BulletinsService>().InRequestScope();
            kernel.Bind<ICustomCommentableService>().To<CustomCommentableService>().InRequestScope();
            

            kernel.Bind<INotifyableService>().To<NewsService>().InRequestScope();
            kernel.Bind<INotifyableService>().To<EventsService>().InRequestScope();
            kernel.Bind<INotifyableService>().To<BulletinsService>().InRequestScope();

            kernel.Bind<ISubscribeService>().To<CustomSubscribeService>().InRequestScope();
            kernel.Bind<IMigrationHistoryService>().To<MigrationHistoryService>().InRequestScope();

            kernel.Bind<IUmbracoContentHelper>().To<UmbracoContentHelper>().InRequestScope();
            kernel.Bind<IIntranetUserContentProvider>().To<IntranetUserContentProvider>().InRequestScope();

            // Navigation 
            kernel.Bind<IConfigurationProvider<NavigationConfiguration>>().To<ConfigurationProvider<NavigationConfiguration>>().InSingletonScope()
                .WithConstructorArgument("settingsFilePath", "~/App_Plugins/Navigation/config/navigationConfiguration.json");

            kernel.Bind<INavigationCompositionService>().To<NavigationCompositionService>().InRequestScope();
            kernel.Bind<IHomeNavigationCompositionService>().To<HomeNavigationCompositionService>().InRequestScope();
            kernel.Bind<ILeftSideNavigationModelBuilder>().To<LeftSideNavigationModelBuilder>().InRequestScope();
            kernel.Bind<ISubNavigationModelBuilder>().To<SubNavigationModelBuilder>().InRequestScope();
            kernel.Bind<ITopNavigationModelBuilder>().To<TopNavigationModelBuilder>().InRequestScope();
            kernel.Bind<IMyLinksModelBuilder>().To<MyLinksModelBuilder>().InRequestScope();
            kernel.Bind<IMyLinksService>().To<MyLinksService>().InRequestScope();
            kernel.Bind<ISystemLinksModelBuilder>().To<SystemLinksModelBuilder>().InRequestScope();
            kernel.Bind<ISystemLinksService>().To<SystemLinksService>().InRequestScope();
            kernel.Bind<IEqualityComparer<MyLinkItemModel>>().To<MyLinkItemModelComparer>().InSingletonScope();
            

            // Notifications
            kernel.Bind<IConfigurationProvider<NotificationConfiguration>>().To<NotificationConfigurationProvider>().InSingletonScope()
                .WithConstructorArgument(typeof(string), "~/App_Plugins/Notification/config/notificationConfiguration.json");
            kernel.Bind<IConfigurationProvider<ReminderConfiguration>>().To<ConfigurationProvider<ReminderConfiguration>>().InSingletonScope()
                .WithConstructorArgument(typeof(string), "~/App_Plugins/Notification/config/reminderConfiguration.json");
            kernel.Bind<INotificationContentProvider>().To<NotificationContentProvider>().InRequestScope();
            kernel.Bind<INotifierService>().To<UiNotifierService>().InRequestScope();
            kernel.Bind<INotifierService>().To<MailNotifierService>().InRequestScope();
            kernel.Bind<INotificationsService>().To<NotificationsService>().InRequestScope();
            kernel.Bind<IUiNotificationService>().To<UiNotificationService>().InRequestScope();
            kernel.Bind<IReminderService>().To<ReminderService>().InRequestScope();
            kernel.Bind<IReminderJob>().To<ReminderJob>().InRequestScope();
            kernel.Bind<IMemberNotifiersSettingsService>().To<MemberNotifiersSettingsService>().InRequestScope();
            kernel.Bind<IMailService>().To<MailService>().InRequestScope();
            kernel.Bind<INotificationSettingsService>().To<NotificationSettingsService>().InRequestScope();
            kernel.Bind<INotificationModelMapper<UiNotifierTemplate,UiNotificationMessage>>().To<UiNotificationModelMapper>().InRequestScope();
            kernel.Bind<INotificationModelMapper<EmailNotifierTemplate,EmailNotificationMessage>>().To<MailNotificationModelMapper>().InRequestScope();

            kernel.Bind<IBackofficeSettingsReader>().To<BackofficeSettingsReader>();
            kernel.Bind(typeof(IBackofficeNotificationSettingsProvider)).To<BackofficeNotificationSettingsProvider>();
            kernel.Bind<INotificationSettingsTreeProvider>().To<NotificationSettingsTreeProvider>();
            kernel.Bind<INotificationSettingCategoryProvider>().To<NotificationSettingCategoryProvider>();
            
            kernel.Bind<IMonthlyEmailService>().To<MonthlyEmailService>().InRequestScope();

            // User tags
            kernel.Bind<IUserTagProvider>().To<UserTagProvider>().InRequestScope();
            kernel.Bind<IUserTagRelationService>().To<UserTagRelationService>().InRequestScope();
            kernel.Bind<IUserTagService>().To<UserTagService>().InRequestScope();

            // Factories
            kernel.Bind<IActivitiesServiceFactory>().To<ActivitiesServiceFactory>().InRequestScope();

            kernel.Bind<IExceptionLogger>().To<ExceptionLogger>().InRequestScope();

            // Model Binders
            kernel.Bind<DateTimeBinder>().ToSelf().InSingletonScope();

            kernel.Bind<IGridHelper>().To<GridHelper>().InRequestScope();

            kernel.Bind<IApplicationSettings>().To<UintraApplicationSettings>().InSingletonScope();
            kernel.Bind<IuIntraApplicationSettings>().To<UintraApplicationSettings>().InSingletonScope();
            kernel.Bind<ISearchApplicationSettings>().To<SearchApplicationSettings>().InSingletonScope();
            kernel.Bind<INavigationApplicationSettings>().To<NavigationApplicationSettings>().InSingletonScope();

            kernel.Bind<IDateTimeFormatProvider>().To<DateTimeFormatProvider>().InRequestScope();
            kernel.Bind<ITimezoneOffsetProvider>().To<TimezoneOffsetProvider>().InRequestScope();
            kernel.Bind<ICookieProvider>().To<CookieProvider>().InRequestScope();

            kernel.Bind<IActivityTypeProvider>().To<ActivityTypeProvider>().InRequestScope();
            kernel.Bind<INotifierTypeProvider>().To<NotifierTypeProvider>().InRequestScope();
            kernel.Bind<IMediaTypeProvider>().To<MediaTypeProvider>().InRequestScope();
            kernel.Bind<IFeedTypeProvider>().To<CentralFeedTypeProvider>().InRequestScope();

            kernel.Bind<IGroupService>().To<GroupService>().InRequestScope();
            kernel.Bind<IGroupMemberService>().To<GroupMemberService>().InRequestScope();
            kernel.Bind<IGroupContentProvider>().To<GroupContentProvider>().InRequestScope();
            kernel.Bind<IGroupLinkProvider>().To<GroupLinkProvider>().InRequestScope();

            kernel.Bind<IGroupMediaService>().To<GroupMediaService>().InRequestScope();
            kernel.Bind<IProfileLinkProvider>().To<ProfileLinkProvider>().InRequestScope();

            kernel.Bind<INotificationTypeProvider>().To<NotificationTypeProvider>().InRequestScope();
            kernel.Bind<ISearchableTypeProvider>().To<SearchableTypeProvider>().InRequestScope();
            kernel.Bind<IMediaFolderTypeProvider>().To<MediaFolderTypeProvider>().InRequestScope();
            kernel.Bind<IIntranetRoleTypeProvider>().To<IntranetRoleTypeProvider>().InRequestScope();


            //umbraco events subscriptions
            kernel.Bind<IUmbracoEventService<IPublishingStrategy, PublishEventArgs<IContent>>>().To<ContentIndexer>().InRequestScope();
            kernel.Bind<IUmbracoEventService<IPublishingStrategy, PublishEventArgs<IContent>>>().To<SearchContentEventService>().InRequestScope();
            kernel.Bind<IUmbracoEventService<IPublishingStrategy, PublishEventArgs<IContent>>>().To<PagePromotionEventService>().InRequestScope();
            kernel.Bind<IUmbracoEventService<IMediaService, SaveEventArgs<IMedia>>>().To<SearchMediaEventService>().InRequestScope();
            kernel.Bind<IUmbracoEventService<IMediaService, MoveEventArgs<IMedia>>>().To<SearchMediaEventService>().InRequestScope();
            kernel.Bind<IUmbracoEventService<IMemberService, DeleteEventArgs<IMember>>>().To<MemberEventService>().InRequestScope();

            kernel.Bind<IUmbracoContentTrashedEventsService>().To<UserTagUmbracoEventHandler>().InRequestScope();
            

            kernel.Bind<IDocumentTypeAliasProvider>().To<DocumentTypeProvider>().InRequestScope();
            kernel.Bind<IXPathProvider>().To<XPathProvider>().InRequestScope();
            
            kernel.Bind<IImageHelper>().To<ImageHelper>().InRequestScope();
            kernel.Bind<INotifierDataHelper>().To<NotifierDataHelper>().InRequestScope();
        }

        private static void RegisterEntityFrameworkServices(IKernel kernel)
        {

            kernel.Bind(typeof(IDbContextFactory<DbObjectContext>)).To<DbContextFactory>().WithConstructorArgument(typeof(string), "umbracoDbDSN");
            kernel.Bind<DbContext>().ToMethod(c => kernel.Get<IDbContextFactory<DbObjectContext>>().Create()).InRequestScope();
            kernel.Bind<IntranetDbContext>().To<DbObjectContext>();
            kernel.Bind<Database>().ToMethod(c => kernel.Get<DbObjectContext>().Database);
            kernel.Bind(typeof(ISqlRepository<,>)).To(typeof(SqlRepository<,>));
            kernel.Bind(typeof(ISqlRepository<>)).To(typeof(SqlRepository<>));
        }

        private static void RegisterGlobalFilters(IKernel kernel)
        {
            GlobalFilters.Filters.Add(new CustomAuthorizeAttribute());
        }

        private static void RegisterLocalizationServices(IKernel kernel)
        {
            kernel.Bind<ILocalizationConfigurationSection>().ToMethod(c => (ILocalizationConfigurationSection)ConfigurationManager.GetSection("localizationConfiguration")).InSingletonScope();
            kernel.Bind<ILocalizationSettingsService>().To<LocalizationSettingsService>().InRequestScope();
            kernel.Bind<ILocalizationCacheProvider>().To<LocalizationMemoryCacheProvider>().InRequestScope();
            kernel.Bind<ILocalizationCacheService>().To<LocalizationCacheService>().InRequestScope();
            kernel.Bind<ILocalizationResourceCacheService>().To<LocalizationResourceCacheService>().InRequestScope();
            kernel.Bind<ILocalizationStorageService>().To<LocalizationStorageService>().InRequestScope();
            kernel.Bind<ILocalizationServiceLanguageEventHandlers>().To<LocalizationServiceLanguageEventHandlers>().InRequestScope();
            kernel.Bind<ILocalizationCoreService>().To<LocalizationCoreService>().InRequestScope();
            kernel.Bind<ILocalizationExportService>().To<LocalizationExportService>().InRequestScope();
            kernel.Bind<ILocalizationImportService>().To<LocalizationImportService>().InRequestScope();

            kernel.Bind<ICultureHelper>().To<CultureHelper>().InRequestScope();
        }

        private static void RegisterSearchServices(IKernel kernel)
        {
            kernel.Bind<IIndexer>().To<NewsService>().InRequestScope();
            kernel.Bind<IIndexer>().To<EventsService>().InRequestScope();
            kernel.Bind<IIndexer>().To<BulletinsService>().InRequestScope();
            kernel.Bind<IIndexer>().To<ContentIndexer>().InRequestScope();
            kernel.Bind<IIndexer>().To<DocumentIndexer>().InRequestScope();
            kernel.Bind<IContentIndexer>().To<ContentIndexer>().InRequestScope();
            kernel.Bind<IDocumentIndexer>().To<DocumentIndexer>().InRequestScope();
            kernel.Bind<IElasticConfigurationSection>().ToMethod(f => ConfigurationManager.GetSection("elasticConfiguration") as ElasticConfigurationSection).InSingletonScope();
            kernel.Bind<IElasticSearchRepository>().To<ElasticSearchRepository>().InRequestScope().WithConstructorArgument(typeof(string), "intranet");
            kernel.Bind(typeof(IElasticSearchRepository<>)).To(typeof(ElasticSearchRepository<>)).InRequestScope().WithConstructorArgument(typeof(string), "intranet");
            kernel.Bind(typeof(PropertiesDescriptor<SearchableActivity>)).To<SearchableActivityMap>().InSingletonScope();
            kernel.Bind(typeof(PropertiesDescriptor<SearchableContent>)).To<SearchableContentMap>().InSingletonScope();
            kernel.Bind(typeof(PropertiesDescriptor<SearchableDocument>)).To<SearchableDocumentMap>().InSingletonScope();
            kernel.Bind<IElasticActivityIndex>().To<ElasticActivityIndex>().InRequestScope();
            kernel.Bind<IElasticContentIndex>().To<ElasticContentIndex>().InRequestScope();
            kernel.Bind<IElasticDocumentIndex>().To<ElasticDocumentIndex>().InRequestScope();
            kernel.Bind<IElasticIndex>().To<ElasticIndex>().InRequestScope();

            kernel.Bind<ISearchUmbracoHelper>().To<SearchUmbracoHelper>().InRequestScope();
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
