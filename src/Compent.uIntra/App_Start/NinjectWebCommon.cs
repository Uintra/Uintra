using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Compent.CommandBus;
using Compent.LinkPreview.HttpClient;
using Compent.Uintra;
using Compent.Uintra.Controllers;
using Compent.Uintra.Core;
using Compent.Uintra.Core.Activity;
using Compent.Uintra.Core.Bulletins;
using Compent.Uintra.Core.CentralFeed;
using Compent.Uintra.Core.Comments;
using Compent.Uintra.Core.ContentPage;
using Compent.Uintra.Core.Controls.EditorConfiguration;
using Compent.Uintra.Core.Events;
using Compent.Uintra.Core.Exceptions;
using Compent.Uintra.Core.Feed.Links;
using Compent.Uintra.Core.Groups;
using Compent.Uintra.Core.Helpers;
using Compent.Uintra.Core.IoC;
using Compent.Uintra.Core.LinkPreview.Config;
using Compent.Uintra.Core.Navigation;
using Compent.Uintra.Core.News;
using Compent.Uintra.Core.Notification;
using Compent.Uintra.Core.PagePromotion;
using Compent.Uintra.Core.Search;
using Compent.Uintra.Core.Search.Entities;
using Compent.Uintra.Core.Search.Entities.Mappings;
using Compent.Uintra.Core.Search.Indexes;
using Compent.Uintra.Core.Subscribe;
using Compent.Uintra.Core.UintraInformation;
using Compent.Uintra.Core.Updater;
using Compent.Uintra.Core.Users;
using Compent.Uintra.Core.Users.RelatedUser;
using Compent.Uintra.Core.UserTags;
using Compent.Uintra.Core.UserTags.Indexers;
using Compent.Uintra.Core.Verification;
using Compent.Uintra.Persistence.Sql;
using EmailWorker.Ninject;
using FluentScheduler;
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
using Ninject.Extensions.Conventions;
using Ninject.Web.Common;
using Ninject.Web.Common.WebHost;
using Uintra.Bulletins;
using Uintra.CentralFeed;
using Uintra.CentralFeed.Providers;
using Uintra.Comments;
using Uintra.Core;
using Uintra.Core.Activity;
using Uintra.Core.ApplicationSettings;
using Uintra.Core.Attributes;
using Uintra.Core.BrowserCompatibility;
using Uintra.Core.Caching;
using Uintra.Core.Configuration;
using Uintra.Core.Controls;
using Uintra.Core.Core.UmbracoEventServices;
using Uintra.Core.Exceptions;
using Uintra.Core.Grid;
using Uintra.Core.Jobs;
using Uintra.Core.Jobs.Configuration;
using Uintra.Core.LinkPreview;
using Uintra.Core.Links;
using Uintra.Core.Localization;
using Uintra.Core.Location;
using Uintra.Core.Media;
using Uintra.Core.MigrationHistories;
using Uintra.Core.ModelBinders;
using Uintra.Core.PagePromotion;
using Uintra.Core.Permissions.Implementation;
using Uintra.Core.Permissions;
using Uintra.Core.Permissions.Interfaces;
using Uintra.Core.Permissions.TypeProviders;
using Uintra.Core.Permissions.UmbracoEvents;
using Uintra.Core.Persistence;
using Uintra.Core.Providers;
using Uintra.Core.TypeProviders;
using Uintra.Core.UmbracoEventServices;
using Uintra.Core.User;
using Uintra.Core.User.Permissions;
using Uintra.Core.Utils;
using Uintra.Events;
using Uintra.Groups;
using Uintra.Groups.Permissions;
using Uintra.Likes;
using Uintra.Navigation;
using Uintra.Navigation.Configuration;
using Uintra.Navigation.Dashboard;
using Uintra.Navigation.EqualityComparers;
using Uintra.Navigation.MyLinks;
using Uintra.Navigation.SystemLinks;
using Uintra.News;
using Uintra.Notification;
using Uintra.Notification.Configuration;
using Uintra.Notification.Jobs;
using Uintra.Panels.Core.PresentationBuilders;
using Uintra.Search;
using Uintra.Search.Configuration;
using Uintra.Subscribe;
using Uintra.Tagging.UserTags;
using Uintra.Users;
using UIntra.Core.Media;
using Umbraco.Core;
using Umbraco.Core.Configuration;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Routing;
using Umbraco.Web.Security;
using MyLinksModelBuilder = Compent.Uintra.Core.Navigation.MyLinksModelBuilder;
using ReminderJob = Uintra.Notification.ReminderJob;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(NinjectWebCommon), "PostStart")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(NinjectWebCommon), "Stop")]

namespace Compent.Uintra
{
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DbObjectContext, Persistence.Sql.Migrations.Configuration>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<EmailWorker.Data.DataDbContext, EmailWorker.Data.Migrations.Configuration>());
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
                RegisterGlobalFilters();
                RegisterLocalizationServices(kernel);
                RegisterSearchServices(kernel);
                RegisterCommandBusServices(kernel);

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
            //migration
            kernel.Bind(x => x.FromAssemblyContaining<IMigration>()
                .SelectAllClasses()
                .InheritedFrom(typeof(IMigration))
                .BindSingleInterface());
            kernel.Bind<IMigrationStepsResolver>().To<MigrationStepsResolver>().InSingletonScope();

            kernel.Bind<IUintraInformationService>().To<UintraInformationService>().InSingletonScope();
  
            //verification
            kernel.Bind<IUmbracoVerificationService>().To<UmbracoVerificationService>().InRequestScope();

            //security

            kernel.Bind<IBrowserCompatibilityConfigurationSection>().ToMethod(s => BrowserCompatibilityConfigurationSection.Configuration).InSingletonScope();
            kernel.Bind<IJobSettingsConfiguration>().ToMethod(s => JobSettingsConfiguration.Configure).InSingletonScope();

            //permissions

            kernel.Bind<IIntranetMemberGroupService>().To<IntranetMemberGroupService>().InSingletonScope();
            kernel.Bind<IPermissionSettingsSchemaProvider>().To<PermissionSettingsSchemaProvider>().InSingletonScope();
            kernel.Bind<IPermissionsService>().To<PermissionsService>().InRequestScope();

            // Umbraco
            kernel.Bind<UmbracoContext>().ToMethod(context => CreateUmbracoContext()).InRequestScope();
            kernel.Bind<UmbracoHelper>().ToSelf().InRequestScope();
            kernel.Bind<IUserService>().ToMethod(i => ApplicationContext.Current.Services.UserService).InRequestScope();
            kernel.Bind<ISectionService>().ToMethod(i => ApplicationContext.Current.Services.SectionService).InRequestScope();
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
            kernel.Bind(typeof(IIntranetUserService<IIntranetUser>)).To<IntranetUserService<IntranetUser>>().InRequestScope();

            kernel.Bind(typeof(IIntranetMemberService<>)).To<IntranetMemberService<IntranetMember>>().InRequestScope();
            kernel.Bind<ICacheableIntranetMemberService>().To<IntranetMemberService<IntranetMember>>().InRequestScope();
            kernel.Bind(typeof(INewsService<>)).To<NewsService>().InRequestScope();
            kernel.Bind(typeof(IEventsService<>)).To<EventsService>().InRequestScope();
            kernel.Bind(typeof(IBulletinsService<>)).To<BulletinsService>().InRequestScope();
            kernel.Bind(typeof(IPagePromotionService<>)).To<PagePromotionService>().InRequestScope();
            kernel.Bind<IMediaHelper>().To<MediaHelper>().InRequestScope();
            kernel.Bind<IVideoConverter>().To<VideoConverter>().InRequestScope();
            kernel.Bind<IVideoConverterLogService>().To<VideoConverterLogService>().InRequestScope();
            kernel.Bind<IIntranetActivityRepository>().To<IntranetActivityRepository>().InRequestScope();
            kernel.Bind<ICacheService>().To<MemoryCacheService>().InRequestScope();
            kernel.Bind<IMemberServiceHelper>().To<MemberServiceHelper>().InRequestScope();
            kernel.Bind<IIntranetMediaService>().To<IntranetMediaService>().InRequestScope();
            kernel.Bind<IEditorConfigProvider>().To<IntranetEditorConfigProvider>().InRequestScope();
            kernel.Bind<IEmbeddedResourceService>().To<EmbeddedResourceService>().InRequestScope();

            kernel.Bind<ICommentsService>().To<CommentsService>().InRequestScope();
            kernel.Bind<ICommentLinkPreviewService>().To<CommentLinkPreviewService>().InRequestScope();
            kernel.Bind<ICommentsPageHelper>().To<CommentsPageHelper>().InRequestScope();
            kernel.Bind<ICommentLinkHelper>().To<CommentLinkHelper>().InRequestScope();

            kernel.Bind<IMentionService>().To<MentionService>().InRequestScope();

            kernel.Bind<ILikesService>().To<LikesService>().InRequestScope();

            // Feed
            kernel.Bind<IFeedItemService>().To<NewsService>().InRequestScope();
            kernel.Bind<IFeedItemService>().To<EventsService>().InRequestScope();
            kernel.Bind<IFeedItemService>().To<BulletinsService>().InRequestScope();
            kernel.Bind<IFeedItemService>().To<PagePromotionService>().InRequestScope();
            kernel.Bind<IFeedFilterService>().To<FeedFilterService>().InRequestScope();

            kernel.Bind<ICentralFeedService>().To<CentralFeedService>().InRequestScope();
            kernel.Bind<IGroupFeedService>().To<GroupFeedService>().InRequestScope();

            kernel.Bind<ICentralFeedLinkProvider>().To<CentralFeedLinkProvider>();
            kernel.Bind<IGroupFeedLinkProvider>().To<GroupFeedLinkProvider>();
            kernel.Bind<IActivityLinkService>().To<ActivityLinkService>();
            kernel.Bind<IFeedLinkService>().To<ActivityLinkService>();

            kernel.Bind<IFeedActivityHelper>().To<FeedActivityHelper>();
            kernel.Bind<IGroupActivityService>().To<GroupActivityService>();
            kernel.Bind<IActivityTypeHelper>().To<ActivityTypeHelper>();

            kernel.Bind<IActivityPageHelperFactory>().To<CacheActivityPageHelperFactory>()
                .WhenInjectedInto<CentralFeedLinkProvider>()
                .WithConstructorArgument(typeof(IEnumerable<string>), c => CentralFeedLinkProviderHelper.GetFeedActivitiesXPath(c.Kernel.Get<IDocumentTypeAliasProvider>()));

            kernel.Bind<IActivityPageHelperFactory>().To<CacheActivityPageHelperFactory>()
                .WhenInjectedInto<EventsController>()
                .WithConstructorArgument(typeof(IEnumerable<string>), c => CentralFeedLinkProviderHelper.GetFeedActivitiesXPath(c.Kernel.Get<IDocumentTypeAliasProvider>()));

            kernel.Bind<IActivityPageHelperFactory>().To<CacheActivityPageHelperFactory>()
                .WhenInjectedInto<GroupFeedLinkProvider>()
                .WithConstructorArgument(typeof(IEnumerable<string>), c => GroupFeedLinkProviderHelper.GetFeedActivitiesXPath(c.Kernel.Get<IDocumentTypeAliasProvider>()));

            kernel.Bind<ICentralFeedContentService>().To<CentralFeedContentService>().InRequestScope();
            kernel.Bind<IGroupFeedContentService>().To<GroupFeedContentService>().InRequestScope();

            kernel.Bind<ICentralFeedContentProvider>().To<CentralFeedContentProvider>().InRequestScope();

            kernel.Bind<ICentralFeedHelper>().To<CentralFeedHelper>().InRequestScope();
            kernel.Bind<IGroupHelper>().To<GroupHelper>().InRequestScope();
            kernel.Bind<IFeedFilterStateService<FeedFiltersState>>().To<CentralFeedFilterStateService>().InRequestScope();

            kernel.Bind(typeof(IIntranetActivityService<>)).To<NewsService>().InRequestScope();
            kernel.Bind(typeof(IIntranetActivityService<>)).To<EventsService>().InRequestScope();
            kernel.Bind(typeof(IIntranetActivityService<>)).To<BulletinsService>().InRequestScope();
            kernel.Bind(typeof(IIntranetActivityService<>)).To<PagePromotionService>().InRequestScope();

            kernel.Bind<IIntranetActivityService>().To<NewsService>().InRequestScope();
            kernel.Bind<IIntranetActivityService>().To<EventsService>().InRequestScope();
            kernel.Bind<IIntranetActivityService>().To<BulletinsService>().InRequestScope();
            kernel.Bind<IIntranetActivityService>().To<PagePromotionService>().InRequestScope();

            kernel.Bind(typeof(ICacheableIntranetActivityService<>)).To<NewsService>().InRequestScope();
            kernel.Bind(typeof(ICacheableIntranetActivityService<>)).To<EventsService>().InRequestScope();
            kernel.Bind(typeof(ICacheableIntranetActivityService<>)).To<BulletinsService>().InRequestScope();
            kernel.Bind(typeof(ICacheableIntranetActivityService<>)).To<PagePromotionService>().InRequestScope();

            kernel.Bind<ISubscribableService>().To<EventsService>().InRequestScope();

            kernel.Bind<INotifyableService>().To<NewsService>().InRequestScope();
            kernel.Bind<INotifyableService>().To<EventsService>().InRequestScope();
            kernel.Bind<INotifyableService>().To<BulletinsService>().InRequestScope();
            kernel.Bind<INotifyableService>().To<ContentPageNotificationService>().InRequestScope();
            kernel.Bind<INotifyableService>().To<PagePromotionNotificationService>().InRequestScope();

            kernel.Bind<ISubscribeService>().To<CustomSubscribeService>().InRequestScope();
            kernel.Bind<IActivitySubscribeSettingService>().To<ActivitySubscribeSettingService>().InRequestScope();
            kernel.Bind<IMigrationHistoryService>().To<MigrationHistoryService>().InRequestScope();

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
            kernel.Bind<IContentPageContentProvider>().To<ContentPageContentProvider>().InSingletonScope();

            // ActivityLocation
            kernel.Bind<IActivityLocationService>().To<ActivityLocationService>();

            // Notifications
            kernel.Bind<IConfigurationProvider<NotificationConfiguration>>().To<NotificationConfigurationProvider>().InSingletonScope()
                .WithConstructorArgument(typeof(string), "~/App_Plugins/Notification/config/notificationConfiguration.json");
            kernel.Bind<IConfigurationProvider<ReminderConfiguration>>().To<ConfigurationProvider<ReminderConfiguration>>().InSingletonScope()
                .WithConstructorArgument(typeof(string), "~/App_Plugins/Notification/config/reminderConfiguration.json");
            kernel.Bind<INotificationContentProvider>().To<NotificationContentProvider>().InRequestScope();
            kernel.Bind<INotifierService>().To<UiNotifierService>().InRequestScope();
            kernel.Bind<INotifierService>().To<PopupNotifierService>().InRequestScope();
            kernel.Bind<INotifierService>().To<MailNotifierService>().InRequestScope();
            kernel.Bind<INotificationsService>().To<NotificationsService>().InRequestScope();
            kernel.Bind<IUiNotificationService>().To<UiNotificationService>().InRequestScope();
            kernel.Bind<IPopupNotificationService>().To<PopupNotificationsService>().InRequestScope();
            kernel.Bind<IReminderService>().To<ReminderService>().InRequestScope();
            kernel.Bind<IReminderJob>().To<ReminderJob>().InRequestScope();
            kernel.Bind<IMemberNotifiersSettingsService>().To<MemberNotifiersSettingsService>().InRequestScope();
            kernel.Bind<IMailService>().To<MailService>().InRequestScope();
            kernel.Bind<INotificationSettingsService>().To<NotificationSettingsService>().InRequestScope();
            kernel.Bind<INotificationModelMapper<UiNotifierTemplate, UiNotificationMessage>>().To<UiNotificationModelMapper>().InRequestScope();
            kernel.Bind<INotificationModelMapper<PopupNotifierTemplate, PopupNotificationMessage>>().To<PopupNotificationModelMapper>().InRequestScope();
            kernel.Bind<INotificationModelMapper<EmailNotifierTemplate, EmailNotificationMessage>>().To<MailNotificationModelMapper>().InRequestScope();
            kernel.Bind<INotificationModelMapper<DesktopNotifierTemplate, DesktopNotificationMessage>>().To<DesktopNotificationModelMapper>().InRequestScope();
            kernel.Bind<IUserMentionNotificationService>().To<UserMentionNotificationService>().InRequestScope();

            kernel.Bind<IBackofficeSettingsReader>().To<BackofficeSettingsReader>();
            kernel.Bind(typeof(IBackofficeNotificationSettingsProvider)).To<BackofficeNotificationSettingsProvider>();
            kernel.Bind<INotificationSettingsTreeProvider>().To<NotificationSettingsTreeProvider>();
            kernel.Bind<INotificationSettingCategoryProvider>().To<NotificationSettingCategoryProvider>();

            kernel.Bind<IMonthlyEmailService>().To<MonthlyEmailService>().InRequestScope();

            // User tags
            kernel.Bind<IUserTagProvider>().To<UserTagProvider>().InRequestScope();
            kernel.Bind<IUserTagRelationService>().To<UserTagRelationService>().InRequestScope();
            kernel.Bind<IUserTagService>().To<UserTagService>().InRequestScope();
            kernel.Bind<IActivityTagsHelper>().To<ActivityTagsHelper>().InRequestScope();

            // Link preview                   
            kernel.Bind<ILinkPreviewClient>().To<LinkPreviewClient>().InRequestScope();
            kernel.Bind<ILinkPreviewConfiguration>().To<LinkPreviewConfiguration>().InRequestScope();
            kernel.Bind<ILinkPreviewUriProvider>().To<LinkPreviewUriProvider>();
            kernel.Bind<ILinkPreviewConfigProvider>().To<LinkPreviewConfigProvider>();
            kernel.Bind<LinkPreviewModelMapper>().ToSelf();
            kernel.Bind<IActivityLinkPreviewService>().To<ActivityLinkPreviewService>();

            // Factories
            kernel.Bind<IActivitiesServiceFactory>().To<ActivitiesServiceFactory>().InRequestScope();

            kernel.Bind<IExceptionLogger>().To<ExceptionLogger>().InRequestScope();

            // Model Binders
            kernel.Bind<DateTimeBinder>().ToSelf().InSingletonScope();

            kernel.Bind<IGridHelper>().To<GridHelper>().InRequestScope();
            kernel.Bind<ViewRenderer>().ToSelf().InRequestScope();

            kernel.Bind<IApplicationSettings>().To<ApplicationSettings>().InSingletonScope();
            kernel.Bind<ISearchApplicationSettings>().To<SearchApplicationSettings>().InSingletonScope();
            kernel.Bind<INavigationApplicationSettings>().To<NavigationApplicationSettings>().InSingletonScope();

            kernel.Bind<IDateTimeFormatProvider>().To<DateTimeFormatProvider>().InRequestScope();
            kernel.Bind<IClientTimezoneProvider>().To<ClientTimezoneProvider>().InRequestScope();
            kernel.Bind<ICookieProvider>().To<CookieProvider>().InRequestScope();

            //type providers

            kernel.Bind<IActivityTypeProvider>().To<ActivityTypeProvider>().InSingletonScope();
            kernel.Bind<INotifierTypeProvider>().To<NotifierTypeProvider>().InSingletonScope();
            kernel.Bind<IMediaTypeProvider>().To<MediaTypeProvider>().InSingletonScope();
            kernel.Bind<IFeedTypeProvider>().To<CentralFeedTypeProvider>().InSingletonScope();
            kernel.Bind<IContextTypeProvider>().To<ContextTypeProvider>().InSingletonScope();
            kernel.Bind<INotificationTypeProvider>().To<NotificationTypeProvider>().InSingletonScope();
            kernel.Bind<ISearchableTypeProvider>().To<UintraSearchableTypeProvider>().InSingletonScope();
            kernel.Bind<IMediaFolderTypeProvider>().To<MediaFolderTypeProvider>().InSingletonScope();
            kernel.Bind<IDocumentTypeAliasProvider>().To<DocumentTypeProvider>().InSingletonScope();
            kernel.Bind<IPermissionActionTypeProvider>().To<PermissionActionTypeProvider>().InSingletonScope(); 
            kernel.Bind<IPermissionResourceTypeProvider>().To<PermissionActivityTypeProvider>().InSingletonScope(); 
            kernel.Bind<IIntranetMemberGroupProvider>().To<IntranetMemberGroupProvider>().InSingletonScope();
            

            kernel.Bind<IGroupService>().To<GroupService>().InRequestScope();
            kernel.Bind<IGroupMemberService>().To<GroupMemberService>().InRequestScope();
            kernel.Bind<IGroupDocumentsService>().To<GroupDocumentsService>().InRequestScope();
            kernel.Bind<IGroupContentProvider>().To<GroupContentProvider>().InRequestScope();
            kernel.Bind<IGroupLinkProvider>().To<GroupLinkProvider>().InRequestScope();

            kernel.Bind<IGroupMediaService>().To<GroupMediaService>().InRequestScope();
            kernel.Bind<IProfileLinkProvider>().To<ProfileLinkProvider>().InRequestScope();

            

            //umbraco events subscriptions
            kernel.Bind<IUmbracoContentPublishedEventService>().To<SearchContentEventService>().InRequestScope();
            kernel.Bind<IUmbracoContentUnPublishedEventService>().To<SearchContentEventService>().InRequestScope();
            kernel.Bind<IUmbracoContentUnPublishedEventService>().To<PagePromotionEventService>().InRequestScope();
            kernel.Bind<IUmbracoContentPublishedEventService>().To<PagePromotionEventService>().InRequestScope();
            kernel.Bind<IUmbracoMediaTrashedEventService>().To<SearchMediaEventService>().InRequestScope();
            kernel.Bind<IUmbracoMediaSavedEventService>().To<SearchMediaEventService>().InRequestScope();
            kernel.Bind<IUmbracoMediaSavingEventService>().To<SearchMediaEventService>().InRequestScope();
            kernel.Bind<IUmbracoMemberDeletingEventService>().To<MemberEventService>().InRequestScope();
            kernel.Bind<IUmbracoContentTrashedEventService>().To<DeleteUserTagHandler>().InRequestScope();
            kernel.Bind<IUmbracoContentPublishedEventService>().To<ContentPageRelationHandler>().InRequestScope();
            kernel.Bind<IUmbracoContentTrashedEventService>().To<ContentPageRelationHandler>().InRequestScope();
            kernel.Bind<IUmbracoContentPublishedEventService>().To<CreateUserTagHandler>().InRequestScope();
            kernel.Bind<IUmbracoContentUnPublishedEventService>().To<CreateUserTagHandler>().InRequestScope();
            kernel.Bind<IUmbracoMediaSavedEventService>().To<VideoConvertEventService>().InRequestScope();
            kernel.Bind<IUmbracoMemberGroupDeletingEventService>().To<UmbracoMemberGroupEventService>().InRequestScope();
            kernel.Bind<IUmbracoMemberGroupSavedEventService>().To<UmbracoMemberGroupEventService>().InRequestScope();
            kernel.Bind<IUmbracoContentSavingEventService>().To<UmbracoContentSavingEventService>().InRequestScope();
            kernel.Bind<IUmbracoMemberCreatedEventService>().To<MemberEventService>().InRequestScope();
            kernel.Bind<IUmbracoMemberAssignedRolesEventService>().To<MemberEventService>().InRequestScope();
            kernel.Bind<IUmbracoMemberRemovedRolesEventService>().To<MemberEventService>().InRequestScope();
            

            kernel.Bind<IXPathProvider>().To<XPathProvider>().InRequestScope();

            kernel.Bind<IImageHelper>().To<ImageHelper>().InRequestScope();
            kernel.Bind<IVideoHelper>().To<VideoHelper>().InRequestScope();
            kernel.Bind<INotifierDataHelper>().To<NotifierDataHelper>().InRequestScope();
            kernel.Bind<INotifierDataBuilder>().To<NotifierDataBuilder>().InRequestScope();

            //Jobs 
            kernel.Bind<global::Uintra.Notification.Jobs.ReminderJob>().ToSelf().InRequestScope();
            kernel.Bind<MontlyMailJob>().ToSelf().InRequestScope();
            kernel.Bind<SendEmailJob>().ToSelf().InRequestScope();
            kernel.Bind<UpdateActivityCacheJob>().ToSelf().InRequestScope();
            kernel.Bind<IJobFactory>().To<IntranetJobFactory>().InRequestScope();

            //table
            kernel.Bind<ITablePanelPresentationBuilder>().To<TablePanelPresentationBuilder>().InRequestScope();

            kernel.Bind<ITableCellBuilder>().To<TableCellBuilder>().InRequestScope();
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

        private static void RegisterGlobalFilters()
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
            kernel.Bind<IIndexer>().To<DocumentIndexer>().InRequestScope();
            kernel.Bind<IIndexer>().To<UserTagsSearchIndexer>().InRequestScope();
            kernel.Bind<IIndexer>().To<UintraContentIndexer>().InRequestScope();
            kernel.Bind<IIndexer>().To<IntranetMemberService<IntranetMember>>().InRequestScope();
            kernel.Bind<IContentIndexer>().To<UintraContentIndexer>().InRequestScope();
            kernel.Bind<IDocumentIndexer>().To<DocumentIndexer>().InRequestScope();
            kernel.Bind<IElasticConfigurationSection>().ToMethod(f => ConfigurationManager.GetSection("elasticConfiguration") as ElasticConfigurationSection).InSingletonScope();
            kernel.Bind<IElasticSearchRepository>().To<ElasticSearchRepository>().InRequestScope().WithConstructorArgument(typeof(string), "intranet");
            kernel.Bind(typeof(IElasticSearchRepository<>)).To(typeof(ElasticSearchRepository<>)).InRequestScope().WithConstructorArgument(typeof(string), "intranet");
            kernel.Bind(typeof(PropertiesDescriptor<SearchableActivity>)).To<SearchableActivityMap>().InSingletonScope();
            kernel.Bind(typeof(PropertiesDescriptor<SearchableUintraActivity>)).To<SearchableUintraActivityMap>().InSingletonScope();
            kernel.Bind(typeof(PropertiesDescriptor<SearchableContent>)).To<SearchableContentMap>().InSingletonScope();
            kernel.Bind(typeof(PropertiesDescriptor<SearchableDocument>)).To<SearchableDocumentMap>().InSingletonScope();
            kernel.Bind(typeof(PropertiesDescriptor<SearchableTag>)).To<SearchableTagMap>().InSingletonScope();
            kernel.Bind(typeof(PropertiesDescriptor<SearchableUser>)).To<SearchableUserMap>().InSingletonScope();
            kernel.Bind<IElasticActivityIndex>().To<ElasticActivityIndex>().InRequestScope();
            kernel.Bind<IElasticUintraActivityIndex>().To<ElasticUintraActivityIndex>().InRequestScope();
            kernel.Bind<IElasticContentIndex>().To<ElasticContentIndex>().InRequestScope();
            kernel.Bind<IElasticDocumentIndex>().To<ElasticDocumentIndex>().InRequestScope();
            kernel.Bind<IElasticTagIndex>().To<ElasticTagIndex>().InRequestScope();
            kernel.Bind<IActivityUserTagIndex>().To<ActivityUserTagIndex>().InRequestScope();
            kernel.Bind<IElasticUserIndex>().To<ElasticUserIndex>().InRequestScope();
            kernel.Bind<IElasticUintraContentIndex>().To<ElasticUintraContentIndex>().InRequestScope();
            kernel.Bind<IUserTagsSearchIndexer>().To<UserTagsSearchIndexer>().InRequestScope();

            kernel.Bind<IElasticEntityMapper>().To<ElasticActivityIndex>().InRequestScope();
            kernel.Bind<IElasticEntityMapper>().To<ElasticContentIndex>().InRequestScope();
            kernel.Bind<IElasticEntityMapper>().To<ElasticDocumentIndex>().InRequestScope();
            kernel.Bind<IElasticEntityMapper>().To<ElasticTagIndex>().InRequestScope();
            kernel.Bind<IElasticEntityMapper>().To<ElasticUserIndex>().InRequestScope();

            kernel.Bind<IElasticIndex>().To<UintraElasticIndex>().InRequestScope();
            kernel.Bind<ISearchScoreProvider>().To<SearchScoreProvider>().InRequestScope();

            kernel.Bind<ISearchUmbracoHelper>().To<SearchUmbracoHelper>().InRequestScope();
        }

        private static void RegisterCommandBusServices(IKernel kernel)
        {
            kernel.Bind<ICommandPublisher>().To<CommandPublisher>();
            kernel.Bind<IInstanceFactory>().To<ReflectionInstanceFactory>();
            kernel.Bind<IBusResolver>().To<BusResolver>();
            kernel.Bind<CommandBus.IDependencyResolver>().To<CommandBusDependencyResolver>();
            kernel.Bind<CommandBindingProviderBase>().To<CommandBusConfiguration>().InSingletonScope();
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
