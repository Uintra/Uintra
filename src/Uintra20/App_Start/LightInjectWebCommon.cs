using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Compent.CommandBus;
using Compent.LinkPreview.HttpClient;
using Compent.Shared.ConfigurationProvider.Json;
using Compent.Shared.DependencyInjection.Contract;
using Compent.Shared.DependencyInjection.LightInject;
using Compent.Shared.Logging.Serilog;
using LightInject;
using Localization.Core;
using Localization.Core.Configuration;
using Localization.Storage.UDictionary;
using Localization.Umbraco;
using Localization.Umbraco.Export;
using Localization.Umbraco.Import;
using Localization.Umbraco.UmbracoEvents;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Newtonsoft.Json.Serialization;
using Ninject.Web.Common;
using Ninject.Web.Common.WebHost;
using UBaseline.Core.Startup;
using Uintra20.App_Start;
using Uintra20.Attributes;
using Uintra20.Core;
using Uintra20.Core.Activity;
using Uintra20.Core.ApplicationSettings;
using Uintra20.Core.Bulletins;
using Uintra20.Core.Caching;
using Uintra20.Core.CentralFeed;
using Uintra20.Core.Comments;
using Uintra20.Core.Grid;
using Uintra20.Core.Groups;
using Uintra20.Core.Helpers;
using Uintra20.Core.Ioc;
using Uintra20.Core.Likes;
using Uintra20.Core.LinkPreview;
using Uintra20.Core.Links;
using Uintra20.Core.Localization;
using Uintra20.Core.Location;
using Uintra20.Core.Media;
using Uintra20.Core.Navigation;
using Uintra20.Core.Notification;
using Uintra20.Core.Notification.Configuration;
using Uintra20.Core.Permissions;
using Uintra20.Core.Permissions.Implementation;
using Uintra20.Core.Permissions.Interfaces;
using Uintra20.Core.Permissions.TypeProviders;
using Uintra20.Core.Tagging.UserTags;
using Uintra20.Core.Tagging.UserTags.Services;
using Uintra20.Core.TypeProviders;
using Uintra20.Core.User;
using Uintra20.Core.User.Entities;
using Uintra20.Core.User.RelatedUser;
using Uintra20.Core.UserTags;
using Uintra20.Core.Utils;
using Uintra20.Persistence;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.WebApi.Filters;
using ConfigurationBuilder = Microsoft.Extensions.Configuration.ConfigurationBuilder;

namespace Uintra20.App_Start
{
    public static class LightInjectWebCommon
    {
        //private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        public static void Start(Composition composition)
        {
            //DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            //DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            ConfigureDependencyResolver(composition);
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DbObjectContext, Persistence.Migrations.Configuration>());
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<EmailWorker.Data.DataDbContext, EmailWorker.Data.Migrations.Configuration>());
            //RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //GlobalConfiguration.Configure((config) =>
            //{
            //    config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //    //config.Filters.Add(new UmbracoIpAccessApiFilter());
            //});
        }

        public static void PostStart()
        {
            //var kernel = bootstrapper.Kernel;

            //UmbracoEventsModule.RegisterEvents();

            //var configurationProvider = kernel.Get<IConfigurationProvider<NavigationConfiguration>>();
            //configurationProvider.Initialize();

            //var notificationConfigurationProvider = kernel.Get<IConfigurationProvider<NotificationConfiguration>>();
            //notificationConfigurationProvider.Initialize();

            //var reminderConfigurationProvider = kernel.Get<IConfigurationProvider<ReminderConfiguration>>();
            //reminderConfigurationProvider.Initialize();
        }

        public static void Stop()
        {
            //bootstrapper.ShutDown();
        }

        private static void ConfigureDependencyResolver(Composition composition)
        {
            var container = composition.Concrete as IServiceContainer;

            var builder = new JsonConfigurationBuilder(new ConfigurationBuilder());
            var configuration = builder
                .AddLogging(UBaselineConfiguration.EnvironmentName)
                .AddUBaselineConfiguration()
                .Build();

            var assembly = typeof(LightInjectWebCommon).Assembly;

            var dependencyCollection = new LightInjectDependencyCollection(container, configuration);
            dependencyCollection.AddLogging()
                .AddUBaseline()
                .RegisterInjectModules(assembly)
                .RegisterMvcControllers(assembly)
                .RegisterApiControllers(assembly)
                .RegisterConverters(assembly);

            dependencyCollection.AddTransient<IHttpModule, HttpApplicationInitializationHttpModule>();

            RegisterEntityFrameworkServices(dependencyCollection);
            RegisterServices(dependencyCollection);
            //RegisterModelBinders();
            //RegisterGlobalFilters();
            RegisterLocalizationServices(dependencyCollection);
            //RegisterSearchServices(kernel);
            RegisterCommandBusServices(dependencyCollection);

            //GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
            //return kernel;
        }

        //private static void RegisterModelBinders()
        //{
        //    ModelBinders.Binders.DefaultBinder = new CustomModelBinder();
        //}

        private static void RegisterServices(LightInjectDependencyCollection dependencyCollection)
        {
            ////migration
            //kernel.Bind(x => x.FromAssemblyContaining<IMigration>()
            //    .SelectAllClasses()
            //    .InheritedFrom(typeof(IMigration))
            //    .BindSingleInterface());
            //kernel.Bind<IMigrationStepsResolver>().To<MigrationStepsResolver>().InSingletonScope();

            //kernel.Bind<IUintraInformationService>().To<UintraInformationService>().InSingletonScope();

            ////verification
            //kernel.Bind<IUmbracoVerificationService>().To<UmbracoVerificationService>().InRequestScope();

            ////security

            //kernel.Bind<IBrowserCompatibilityConfigurationSection>().ToMethod(s => BrowserCompatibilityConfigurationSection.Configuration).InSingletonScope();
            //kernel.Bind<IJobSettingsConfiguration>().ToMethod(s => JobSettingsConfiguration.Configure).InSingletonScope();

            //permissions

            dependencyCollection.AddSingleton<IIntranetMemberGroupService, IntranetMemberGroupService>();
            dependencyCollection.AddSingleton<IPermissionSettingsSchemaProvider, PermissionSettingsSchemaProvider>();
            dependencyCollection.AddScoped<IPermissionsService, PermissionsService>();

            //// Umbraco
            //kernel.Bind<UmbracoContext>().ToMethod(context => CreateUmbracoContext()).InRequestScope();
            //kernel.Bind<UmbracoHelper>().ToSelf().InRequestScope();
            //kernel.Bind<IUserService>().ToMethod(i => ApplicationContext.Current.Services.UserService).InRequestScope();
            //kernel.Bind<ISectionService>().ToMethod(i => ApplicationContext.Current.Services.SectionService).InRequestScope();
            //kernel.Bind<IContentService>().ToMethod(i => ApplicationContext.Current.Services.ContentService).InRequestScope();
            //kernel.Bind<IContentTypeService>().ToMethod(i => ApplicationContext.Current.Services.ContentTypeService).InRequestScope();
            //kernel.Bind<IMediaService>().ToMethod(i => ApplicationContext.Current.Services.MediaService).InRequestScope();
            //kernel.Bind<DatabaseContext>().ToMethod(i => ApplicationContext.Current.DatabaseContext).InRequestScope();
            //kernel.Bind<IDataTypeService>().ToMethod(i => ApplicationContext.Current.Services.DataTypeService).InRequestScope();
            //kernel.Bind<IMemberService>().ToMethod(i => ApplicationContext.Current.Services.MemberService).InRequestScope();
            //kernel.Bind<IMemberTypeService>().ToMethod(i => ApplicationContext.Current.Services.MemberTypeService).InRequestScope();
            //kernel.Bind<IMemberGroupService>().ToMethod(i => ApplicationContext.Current.Services.MemberGroupService).InRequestScope();
            //kernel.Bind<ILocalizationService>().ToMethod(i => ApplicationContext.Current.Services.LocalizationService).InRequestScope();
            //kernel.Bind<IDomainService>().ToMethod(i => ApplicationContext.Current.Services.DomainService).InRequestScope();

            // Plugin services
            dependencyCollection.AddScoped<IIntranetLocalizationService, LocalizationService>();
            dependencyCollection.AddScoped<IIntranetUserService<IIntranetUser>, IntranetUserService<IntranetUser>>();

            dependencyCollection.AddScoped(typeof(IIntranetMemberService<>), typeof(IntranetMemberService<>));
            dependencyCollection.AddScoped<ICacheableIntranetMemberService, IntranetMemberService<IntranetMember>>();
            //kernel.Bind(typeof(INewsService<>)).To<NewsService>().InRequestScope();
            //kernel.Bind(typeof(IEventsService<>)).To<EventsService>().InRequestScope();
            dependencyCollection.AddScoped(typeof(IBulletinsService<Bulletin>), typeof(BulletinsService<Bulletin>));
            //kernel.Bind(typeof(IPagePromotionService<>)).To<PagePromotionService>().InRequestScope();
            dependencyCollection.AddScoped<IMediaHelper, MediaHelper>();
            dependencyCollection.AddScoped<IVideoConverter, VideoConverter>();
            dependencyCollection.AddScoped<IVideoConverterLogService, VideoConverterLogService>();
            dependencyCollection.AddScoped<IIntranetActivityRepository, IntranetActivityRepository>();
            dependencyCollection.AddScoped<ICacheService, MemoryCacheService>();
            //kernel.Bind<IMemberServiceHelper>().To<MemberServiceHelper>().InRequestScope();
            dependencyCollection.AddScoped<IIntranetMediaService, IntranetMediaService>();
            //kernel.Bind<IEditorConfigProvider>().To<IntranetEditorConfigProvider>().InRequestScope();
            dependencyCollection.AddScoped<IEmbeddedResourceService, EmbeddedResourceService>();

            dependencyCollection.AddScoped<ICommentsService, CommentsService>();
            dependencyCollection.AddScoped<ICommentLinkPreviewService, CommentLinkPreviewService>();
            //kernel.Bind<ICommentsPageHelper>().To<CommentsPageHelper>().InRequestScope();
            dependencyCollection.AddScoped<ICommentLinkHelper, CommentLinkHelper>();

            dependencyCollection.AddScoped<IMentionService, MentionService>();

            dependencyCollection.AddScoped<ILikesService, LikesService>();

            // Feed
            //kernel.Bind<IFeedItemService>().To<NewsService>().InRequestScope();
            //kernel.Bind<IFeedItemService>().To<EventsService>().InRequestScope();
            dependencyCollection.AddScoped<IFeedItemService, BulletinsService<Bulletin>>();
            //kernel.Bind<IFeedItemService>().To<PagePromotionService>().InRequestScope();
            //kernel.Bind<IFeedFilterService>().To<FeedFilterService>().InRequestScope();

            //kernel.Bind<ICentralFeedService>().To<CentralFeedService>().InRequestScope();
            //kernel.Bind<IGroupFeedService>().To<GroupFeedService>().InRequestScope();

            dependencyCollection.AddTransient<ICentralFeedLinkProvider, CentralFeedLinkProvider>();
            dependencyCollection.AddTransient<IGroupFeedLinkProvider, GroupFeedLinkProvider>();
            dependencyCollection.AddTransient<IActivityLinkService, ActivityLinkService>();
            dependencyCollection.AddTransient<IFeedLinkService, ActivityLinkService>();

            //kernel.Bind<IFeedActivityHelper>().To<FeedActivityHelper>();
            dependencyCollection.AddTransient<IGroupActivityService, GroupActivityService>();
            dependencyCollection.AddTransient<IActivityTypeHelper, ActivityTypeHelper>();

            dependencyCollection.AddTransient<IActivityPageHelperFactory>(provider =>
                new CacheActivityPageHelperFactory(provider.GetService<UmbracoHelper>(),
                    provider.GetService<IDocumentTypeAliasProvider>(),
                    CentralFeedLinkProviderHelper.GetFeedActivitiesXPath(provider.GetService<IDocumentTypeAliasProvider>())));

            //kernel.Bind<IActivityPageHelperFactory>().To<CacheActivityPageHelperFactory>()
            //    .WhenInjectedInto<CentralFeedLinkProvider>()
            //    .WithConstructorArgument(typeof(IEnumerable<string>), c => CentralFeedLinkProviderHelper.GetFeedActivitiesXPath(c.Kernel.Get<IDocumentTypeAliasProvider>()));

            //kernel.Bind<IActivityPageHelperFactory>().To<CacheActivityPageHelperFactory>()
            //    .WhenInjectedInto<EventsController>()
            //    .WithConstructorArgument(typeof(IEnumerable<string>), c => CentralFeedLinkProviderHelper.GetFeedActivitiesXPath(c.Kernel.Get<IDocumentTypeAliasProvider>()));

            //kernel.Bind<IActivityPageHelperFactory>().To<CacheActivityPageHelperFactory>()
            //    .WhenInjectedInto<GroupFeedLinkProvider>()
            //    .WithConstructorArgument(typeof(IEnumerable<string>), c => GroupFeedLinkProviderHelper.GetFeedActivitiesXPath(c.Kernel.Get<IDocumentTypeAliasProvider>()));

            //kernel.Bind<ICentralFeedContentService>().To<CentralFeedContentService>().InRequestScope();
            //kernel.Bind<IGroupFeedContentService>().To<GroupFeedContentService>().InRequestScope();

            //kernel.Bind<ICentralFeedContentProvider>().To<CentralFeedContentProvider>().InRequestScope();

            //kernel.Bind<ICentralFeedHelper>().To<CentralFeedHelper>().InRequestScope();
            //kernel.Bind<IGroupHelper>().To<GroupHelper>().InRequestScope();
            //kernel.Bind<IFeedFilterStateService<FeedFiltersState>>().To<CentralFeedFilterStateService>().InRequestScope();

            //kernel.Bind(typeof(IIntranetActivityService<>)).To<NewsService>().InRequestScope();
            //kernel.Bind(typeof(IIntranetActivityService<>)).To<EventsService>().InRequestScope();
            dependencyCollection.AddScoped(typeof(IIntranetActivityService<Bulletin>), typeof(BulletinsService<Bulletin>));
            //kernel.Bind(typeof(IIntranetActivityService<>)).To<PagePromotionService>().InRequestScope();

            //kernel.Bind<IIntranetActivityService>().To<NewsService>().InRequestScope();
            //kernel.Bind<IIntranetActivityService>().To<EventsService>().InRequestScope();
            dependencyCollection.AddScoped<IIntranetActivityService, BulletinsService<Bulletin>>();
            //kernel.Bind<IIntranetActivityService>().To<PagePromotionService>().InRequestScope();

            //kernel.Bind(typeof(ICacheableIntranetActivityService<>)).To<NewsService>().InRequestScope();
            //kernel.Bind(typeof(ICacheableIntranetActivityService<>)).To<EventsService>().InRequestScope();
            dependencyCollection.AddScoped(typeof(ICacheableIntranetActivityService<Bulletin>), typeof(BulletinsService<Bulletin>));
            //kernel.Bind(typeof(ICacheableIntranetActivityService<>)).To<PagePromotionService>().InRequestScope();

            //kernel.Bind<ISubscribableService>().To<EventsService>().InRequestScope();

            //kernel.Bind<INotifyableService>().To<NewsService>().InRequestScope();
            //kernel.Bind<INotifyableService>().To<EventsService>().InRequestScope();
            dependencyCollection.AddScoped<INotifyableService, BulletinsService<Bulletin>>();
            //kernel.Bind<INotifyableService>().To<ContentPageNotificationService>().InRequestScope();
            //kernel.Bind<INotifyableService>().To<PagePromotionNotificationService>().InRequestScope();

            //kernel.Bind<ISubscribeService>().To<CustomSubscribeService>().InRequestScope();
            //kernel.Bind<IActivitySubscribeSettingService>().To<ActivitySubscribeSettingService>().InRequestScope();
            //kernel.Bind<IMigrationHistoryService>().To<MigrationHistoryService>().InRequestScope();

            dependencyCollection.AddScoped<IIntranetUserContentProvider, IntranetUserContentProvider>();

            // Navigation 
            //kernel.Bind<IConfigurationProvider<NavigationConfiguration>>().To<ConfigurationProvider<NavigationConfiguration>>().InSingletonScope()
            //    .WithConstructorArgument("settingsFilePath", "~/App_Plugins/Navigation/config/navigationConfiguration.json");

            //kernel.Bind<INavigationCompositionService>().To<NavigationCompositionService>().InRequestScope();
            //kernel.Bind<IHomeNavigationCompositionService>().To<HomeNavigationCompositionService>().InRequestScope();
            //kernel.Bind<ILeftSideNavigationModelBuilder>().To<LeftSideNavigationModelBuilder>().InRequestScope();
            //kernel.Bind<ISubNavigationModelBuilder>().To<SubNavigationModelBuilder>().InRequestScope();
            //kernel.Bind<ITopNavigationModelBuilder>().To<TopNavigationModelBuilder>().InRequestScope();
            //kernel.Bind<IMyLinksModelBuilder>().To<MyLinksModelBuilder>().InRequestScope();
            dependencyCollection.AddScoped<IMyLinksService, MyLinksService>();
            //kernel.Bind<ISystemLinksModelBuilder>().To<SystemLinksModelBuilder>().InRequestScope();
            //kernel.Bind<ISystemLinksService>().To<SystemLinksService>().InRequestScope();
            //kernel.Bind<IEqualityComparer<MyLinkItemModel>>().To<MyLinkItemModelComparer>().InSingletonScope();
            //kernel.Bind<IContentPageContentProvider>().To<ContentPageContentProvider>().InSingletonScope();

            // ActivityLocation
            dependencyCollection.AddTransient<IActivityLocationService, ActivityLocationService>();

            // Notifications
            //kernel.Bind<IConfigurationProvider<NotificationConfiguration>>().To<NotificationConfigurationProvider>().InSingletonScope()
            //    .WithConstructorArgument(typeof(string), "~/App_Plugins/Notification/config/notificationConfiguration.json");
            //kernel.Bind<IConfigurationProvider<ReminderConfiguration>>().To<ConfigurationProvider<ReminderConfiguration>>().InSingletonScope()
            //    .WithConstructorArgument(typeof(string), "~/App_Plugins/Notification/config/reminderConfiguration.json");
            //kernel.Bind<INotificationContentProvider>().To<NotificationContentProvider>().InRequestScope();
            dependencyCollection.AddScoped<INotifierService, UiNotifierService>();
            dependencyCollection.AddScoped<INotifierService, PopupNotifierService>();
            dependencyCollection.AddScoped<INotifierService, MailNotifierService>();
            dependencyCollection.AddScoped<INotificationsService, NotificationsService>();
            dependencyCollection.AddScoped<IUiNotificationService, UiNotificationService>();
            dependencyCollection.AddScoped<IPopupNotificationService, PopupNotificationsService>();
            //kernel.Bind<IReminderService>().To<ReminderService>().InRequestScope();
            //kernel.Bind<IReminderJob>().To<ReminderJob>().InRequestScope();
            dependencyCollection.AddScoped<IMemberNotifiersSettingsService, MemberNotifiersSettingsService>();
            dependencyCollection.AddScoped<IMailService, MailService>();
            dependencyCollection.AddTransient<INotificationSettingsService, NotificationSettingsService>();
            dependencyCollection.AddScoped<INotificationModelMapper<UiNotifierTemplate, UiNotificationMessage>, UiNotificationModelMapper>();
            dependencyCollection.AddScoped<INotificationModelMapper<PopupNotifierTemplate, PopupNotificationMessage>, PopupNotificationModelMapper>();
            dependencyCollection.AddScoped<INotificationModelMapper<EmailNotifierTemplate, EmailNotificationMessage>, MailNotificationModelMapper>();
            dependencyCollection.AddScoped<INotificationModelMapper<DesktopNotifierTemplate, DesktopNotificationMessage>, DesktopNotificationModelMapper>();
            //kernel.Bind<IUserMentionNotificationService>().To<UserMentionNotificationService>().InRequestScope();

            dependencyCollection.AddTransient<IBackofficeSettingsReader, BackofficeSettingsReader>();
            dependencyCollection.AddTransient(typeof(IBackofficeNotificationSettingsProvider), typeof(BackofficeNotificationSettingsProvider));
            //kernel.Bind<INotificationSettingsTreeProvider>().To<NotificationSettingsTreeProvider>();
            //kernel.Bind<INotificationSettingCategoryProvider>().To<NotificationSettingCategoryProvider>();

            //kernel.Bind<IMonthlyEmailService>().To<MonthlyEmailService>().InRequestScope();

            // User tags
            dependencyCollection.AddScoped<IUserTagProvider, UserTagProvider>();
            dependencyCollection.AddScoped<IUserTagRelationService, UserTagRelationService>();
            dependencyCollection.AddScoped<IUserTagService, UserTagService>();
            dependencyCollection.AddScoped<IActivityTagsHelper, ActivityTagsHelper>();

            // Link preview                   
            dependencyCollection.AddScoped<ILinkPreviewClient, LinkPreviewClient>();
            //kernel.Bind<ILinkPreviewConfiguration>().To<LinkPreviewConfiguration>().InRequestScope();
            dependencyCollection.AddTransient<ILinkPreviewUriProvider, LinkPreviewUriProvider>();
            //kernel.Bind<ILinkPreviewConfigProvider>().To<LinkPreviewConfigProvider>();
            dependencyCollection.AddTransient(typeof(LinkPreviewModelMapper));
            dependencyCollection.AddTransient<IActivityLinkPreviewService, ActivityLinkPreviewService>();

            // Factories
            dependencyCollection.AddScoped<IActivitiesServiceFactory, ActivitiesServiceFactory>();

            //kernel.Bind<IExceptionLogger>().To<ExceptionLogger>().InRequestScope();

            // Model Binders
            //kernel.Bind<DateTimeBinder>().ToSelf().InSingletonScope();

            dependencyCollection.AddScoped<IGridHelper, GridHelper>();
            //kernel.Bind<ViewRenderer>().ToSelf().InRequestScope();

            dependencyCollection.AddSingleton<IApplicationSettings, ApplicationSettings>();
            //kernel.Bind<ISearchApplicationSettings>().To<SearchApplicationSettings>().InSingletonScope();
            //kernel.Bind<INavigationApplicationSettings>().To<NavigationApplicationSettings>().InSingletonScope();

            dependencyCollection.AddScoped<IDateTimeFormatProvider, DateTimeFormatProvider>();
            dependencyCollection.AddScoped<IClientTimezoneProvider, ClientTimezoneProvider>();
            dependencyCollection.AddScoped<ICookieProvider, CookieProvider>();

            //type providers

            dependencyCollection.AddScoped<IActivityTypeProvider>(provider => new ActivityTypeProvider(typeof(IntranetActivityTypeEnum)));
            dependencyCollection.AddScoped<INotifierTypeProvider>(provider => new NotifierTypeProvider(typeof(NotifierTypeEnum)));
            dependencyCollection.AddScoped<IMediaFolderTypeProvider>(provider => new MediaFolderTypeProvider(typeof(MediaFolderTypeEnum)));
            dependencyCollection.AddScoped<IPermissionActionTypeProvider>(provider => new PermissionActionTypeProvider(typeof(PermissionActionEnum)));
            dependencyCollection.AddScoped<IPermissionResourceTypeProvider>(provider => new PermissionActivityTypeProvider(typeof(PermissionResourceTypeEnum)));

            dependencyCollection.AddSingleton<IDocumentTypeAliasProvider, DocumentTypeProvider>();
            dependencyCollection.AddScoped<IIntranetMemberGroupProvider, IntranetMemberGroupProvider>();


            dependencyCollection.AddScoped<IGroupService, GroupService>();
            //kernel.Bind<IGroupMemberService>().To<GroupMemberService>().InRequestScope();
            //kernel.Bind<IGroupDocumentsService>().To<GroupDocumentsService>().InRequestScope();
            //kernel.Bind<IGroupContentProvider>().To<GroupContentProvider>().InRequestScope();
            //kernel.Bind<IGroupLinkProvider>().To<GroupLinkProvider>().InRequestScope();

            //kernel.Bind<IGroupMediaService>().To<GroupMediaService>().InRequestScope();
            dependencyCollection.AddScoped<IProfileLinkProvider, ProfileLinkProvider>();



            //umbraco events subscriptions
            //kernel.Bind<IUmbracoContentPublishedEventService>().To<SearchContentEventService>().InRequestScope();
            //kernel.Bind<IUmbracoContentUnPublishedEventService>().To<SearchContentEventService>().InRequestScope();
            //kernel.Bind<IUmbracoContentUnPublishedEventService>().To<PagePromotionEventService>().InRequestScope();
            //kernel.Bind<IUmbracoContentPublishedEventService>().To<PagePromotionEventService>().InRequestScope();
            //kernel.Bind<IUmbracoMediaTrashedEventService>().To<SearchMediaEventService>().InRequestScope();
            //kernel.Bind<IUmbracoMediaSavedEventService>().To<SearchMediaEventService>().InRequestScope();
            //kernel.Bind<IUmbracoMediaSavingEventService>().To<SearchMediaEventService>().InRequestScope();
            //kernel.Bind<IUmbracoMemberDeletingEventService>().To<MemberEventService>().InRequestScope();
            //kernel.Bind<IUmbracoContentTrashedEventService>().To<DeleteUserTagHandler>().InRequestScope();
            //kernel.Bind<IUmbracoContentPublishedEventService>().To<ContentPageRelationHandler>().InRequestScope();
            //kernel.Bind<IUmbracoContentTrashedEventService>().To<ContentPageRelationHandler>().InRequestScope();
            //kernel.Bind<IUmbracoContentPublishedEventService>().To<CreateUserTagHandler>().InRequestScope();
            //kernel.Bind<IUmbracoContentUnPublishedEventService>().To<CreateUserTagHandler>().InRequestScope();
            //kernel.Bind<IUmbracoMediaSavedEventService>().To<VideoConvertEventService>().InRequestScope();
            //kernel.Bind<IUmbracoMemberGroupDeletingEventService>().To<UmbracoMemberGroupEventService>().InRequestScope();
            //kernel.Bind<IUmbracoMemberGroupSavedEventService>().To<UmbracoMemberGroupEventService>().InRequestScope();
            //kernel.Bind<IUmbracoContentSavingEventService>().To<UmbracoContentSavingEventService>().InRequestScope();
            //kernel.Bind<IUmbracoMemberCreatedEventService>().To<MemberEventService>().InRequestScope();
            //kernel.Bind<IUmbracoMemberAssignedRolesEventService>().To<MemberEventService>().InRequestScope();
            //kernel.Bind<IUmbracoMemberRemovedRolesEventService>().To<MemberEventService>().InRequestScope();


            dependencyCollection.AddScoped<IXPathProvider, XPathProvider>();

            dependencyCollection.AddScoped<IImageHelper, ImageHelper>();
            dependencyCollection.AddScoped<IVideoHelper, VideoHelper>();
            dependencyCollection.AddScoped<INotifierDataHelper, NotifierDataHelper>();
            dependencyCollection.AddScoped<INotifierDataBuilder, NotifierDataBuilder>();

            //Jobs 
            //kernel.Bind<global::Uintra.Notification.Jobs.ReminderJob>().ToSelf().InRequestScope();
            //kernel.Bind<MontlyMailJob>().ToSelf().InRequestScope();
            //kernel.Bind<SendEmailJob>().ToSelf().InRequestScope();
            //kernel.Bind<UpdateActivityCacheJob>().ToSelf().InRequestScope();
            //kernel.Bind<IJobFactory>().To<IntranetJobFactory>().InRequestScope();

            //table
            //kernel.Bind<ITablePanelPresentationBuilder>().To<TablePanelPresentationBuilder>().InRequestScope();
            //kernel.Bind<ITableCellBuilder>().To<TableCellBuilder>().InRequestScope();

            //UmbracoIpAccess
            //kernel.Bind<IUmbracoIpAccessConfiguration>().ToMethod(f => ConfigurationManager.GetSection("umbracoIpAccessConfiguration") as UmbracoIpAccessConfiguration).InSingletonScope();
            //kernel.Bind<IUmbracoIpAccessValidator>().To<UmbracoIpAccessValidator>().InRequestScope();

            //Open Graph
            //kernel.Bind<IOpenGraphService>().To<OpenGraphService>().InRequestScope();
        }

        private static void RegisterEntityFrameworkServices(LightInjectDependencyCollection dependencyCollection)
        {
            dependencyCollection.AddTransient<IDbContextFactory<DbObjectContext>>(provider => new DbContextFactory("umbracoDbDSN"));
            dependencyCollection.AddScoped<DbContext>(provider => provider.GetService<IDbContextFactory<DbObjectContext>>().Create());
            dependencyCollection.AddTransient<IntranetDbContext, DbObjectContext>();
            dependencyCollection.AddTransient<Database>(provider => provider.GetService<DbObjectContext>().Database);
            dependencyCollection.AddTransient(typeof(ISqlRepository<,>), typeof(SqlRepository<,>));
            dependencyCollection.AddTransient(typeof(ISqlRepository<>), typeof(SqlRepository<>));
        }

        private static void RegisterGlobalFilters()
        {
            GlobalFilters.Filters.Add(new CustomAuthorizeAttribute());
            GlobalFilters.Filters.Add(new FeatureAuthorizeAttribute());
            //GlobalFilters.Filters.Add(new UmbracoIpAccessMvcFilter());
        }

        private static void RegisterLocalizationServices(LightInjectDependencyCollection dependencyCollection)
        {
            dependencyCollection.AddSingleton<ILocalizationConfigurationSection>(provider => (ILocalizationConfigurationSection)ConfigurationManager.GetSection("localizationConfiguration"));
            dependencyCollection.AddScoped<ILocalizationSettingsService, LocalizationSettingsService>();
            dependencyCollection.AddScoped<ILocalizationCacheProvider, LocalizationMemoryCacheProvider>();
            dependencyCollection.AddScoped<ILocalizationCacheService, LocalizationCacheService>();
            dependencyCollection.AddScoped<ILocalizationResourceCacheService, LocalizationResourceCacheService>();
            dependencyCollection.AddScoped<ILocalizationStorageService, LocalizationStorageService>();
            dependencyCollection.AddScoped<ILocalizationServiceLanguageEventHandlers, LocalizationServiceLanguageEventHandlers>();
            dependencyCollection.AddScoped<ILocalizationCoreService, LocalizationCoreService>();
            dependencyCollection.AddScoped<ILocalizationExportService, LocalizationExportService>();
            dependencyCollection.AddScoped<ILocalizationImportService, LocalizationImportService>();

            dependencyCollection.AddScoped<ICultureHelper, CultureHelper>();
        }

        //private static void RegisterSearchServices(IKernel kernel)
        //{
        //    kernel.Bind<IIndexer>().To<NewsService>().InRequestScope();
        //    kernel.Bind<IIndexer>().To<EventsService>().InRequestScope();
        //    kernel.Bind<IIndexer>().To<BulletinsService>().InRequestScope();
        //    kernel.Bind<IIndexer>().To<DocumentIndexer>().InRequestScope();
        //    kernel.Bind<IIndexer>().To<UserTagsSearchIndexer>().InRequestScope();
        //    kernel.Bind<IIndexer>().To<UintraContentIndexer>().InRequestScope();
        //    kernel.Bind<IIndexer>().To<MembersIndexer>().InRequestScope();
        //    kernel.Bind<IContentIndexer>().To<UintraContentIndexer>().InRequestScope();
        //    kernel.Bind<ISearchableMemberMapper>().To<SearchableMemberMapper>().InRequestScope();
        //    kernel.Bind<IDocumentIndexer>().To<DocumentIndexer>().InRequestScope();
        //    kernel.Bind<IElasticConfigurationSection>().ToMethod(f => ConfigurationManager.GetSection("elasticConfiguration") as ElasticConfigurationSection).InSingletonScope();
        //    kernel.Bind<IElasticSearchRepository>().To<ElasticSearchRepository>().InRequestScope().WithConstructorArgument(typeof(string), "intranet");
        //    kernel.Bind(typeof(IElasticSearchRepository<>)).To(typeof(ElasticSearchRepository<>)).InRequestScope().WithConstructorArgument(typeof(string), "intranet");
        //    kernel.Bind(typeof(PropertiesDescriptor<SearchableActivity>)).To<SearchableActivityMap>().InSingletonScope();
        //    kernel.Bind(typeof(PropertiesDescriptor<SearchableUintraActivity>)).To<SearchableUintraActivityMap>().InSingletonScope();
        //    kernel.Bind(typeof(PropertiesDescriptor<SearchableContent>)).To<SearchableContentMap>().InSingletonScope();
        //    kernel.Bind(typeof(PropertiesDescriptor<SearchableDocument>)).To<SearchableDocumentMap>().InSingletonScope();
        //    kernel.Bind(typeof(PropertiesDescriptor<SearchableTag>)).To<SearchableTagMap>().InSingletonScope();
        //    kernel.Bind(typeof(PropertiesDescriptor<SearchableMember>)).To<SearchableUserMap>().InSingletonScope();
        //    kernel.Bind<IElasticActivityIndex>().To<ElasticActivityIndex>().InRequestScope();
        //    kernel.Bind<IElasticUintraActivityIndex>().To<ElasticUintraActivityIndex>().InRequestScope();
        //    kernel.Bind<IElasticContentIndex>().To<ElasticContentIndex>().InRequestScope();
        //    kernel.Bind<IElasticDocumentIndex>().To<ElasticDocumentIndex>().InRequestScope();
        //    kernel.Bind<IElasticTagIndex>().To<ElasticTagIndex>().InRequestScope();
        //    kernel.Bind<IActivityUserTagIndex>().To<ActivityUserTagIndex>().InRequestScope();
        //    kernel.Bind<IElasticMemberIndex>().To<ElasticMemberIndex>().InRequestScope();
        //    kernel.Bind<IElasticUintraContentIndex>().To<ElasticUintraContentIndex>().InRequestScope();
        //    kernel.Bind<IUserTagsSearchIndexer>().To<UserTagsSearchIndexer>().InRequestScope();

        //    kernel.Bind<IElasticEntityMapper>().To<ElasticActivityIndex>().InRequestScope();
        //    kernel.Bind<IElasticEntityMapper>().To<ElasticContentIndex>().InRequestScope();
        //    kernel.Bind<IElasticEntityMapper>().To<ElasticDocumentIndex>().InRequestScope();
        //    kernel.Bind<IElasticEntityMapper>().To<ElasticTagIndex>().InRequestScope();
        //    kernel.Bind<IElasticEntityMapper>().To<ElasticMemberIndex>().InRequestScope();

        //    kernel.Bind<IElasticIndex>().To<UintraElasticIndex>().InRequestScope();
        //    kernel.Bind<IMemberSearchDescriptorBuilder>().To<MemberSearchDescriptorBuilder>().InRequestScope();
        //    kernel.Bind(typeof(ISearchSortingHelper<>)).To(typeof(BaseSearchSortingHelper<>)).InRequestScope();
        //    kernel.Bind(typeof(ISearchPagingHelper<>)).To(typeof(BaseSearchPagingHelper<>)).InRequestScope();
        //    kernel.Bind<ISearchScoreProvider>().To<SearchScoreProvider>().InRequestScope();

        //    kernel.Bind<ISearchUmbracoHelper>().To<SearchUmbracoHelper>().InRequestScope();
        //}

        private static void RegisterCommandBusServices(LightInjectDependencyCollection dependencyCollection)
        {
            dependencyCollection.AddTransient<ICommandPublisher, CommandPublisher>();
            dependencyCollection.AddTransient<IInstanceFactory, ReflectionInstanceFactory>();
            dependencyCollection.AddTransient<IBusResolver, BusResolver>();
            dependencyCollection.AddTransient<Compent.CommandBus.IDependencyResolver, CommandBusDependencyResolver>();
            dependencyCollection.AddSingleton<CommandBindingProviderBase, CommandBusConfiguration>();
        }

        //private static UmbracoContext CreateUmbracoContext()
        //{
        //    var context = HttpContext.Current ?? new HttpContext(new HttpRequest("", "http://localhost/", ""), new HttpResponse(null));
        //    var httpContextWrapper = new HttpContextWrapper(context);
        //    var umbracoSettings = UmbracoConfig.For.UmbracoSettings();
        //    var urlProvider = UrlProviderResolver.Current.Providers;
        //    var webSecurity = new WebSecurity(httpContextWrapper, ApplicationContext.Current);
        //    var result = (httpContextWrapper, ApplicationContext.Current, webSecurity, umbracoSettings, urlProvider, false);
        //    return result;
        //}
    }
}