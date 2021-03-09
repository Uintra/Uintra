using Compent.Shared.DependencyInjection.Contract;
using Localization.Core;
using Localization.Core.Configuration;
using Localization.Storage.UDictionary;
using System.Configuration;
using UBaseline.Core.RequestContext;
using Uintra.Core.Authentication;
using Uintra.Core.Controls.LightboxGallery;
using Uintra.Core.Localization;
using Uintra.Core.Updater;
using Uintra.Core.Updater._2._0;
using Uintra.Features.Information;
using Uintra.Features.Media.Enums;
using Uintra.Features.Media.Helpers;
using Uintra.Features.Media.Images.Helpers.Contracts;
using Uintra.Features.Media.Images.Helpers.Implementations;
using Uintra.Features.Media.Intranet.Services.Contracts;
using Uintra.Features.Media.Intranet.Services.Implementations;
using Uintra.Features.Media.Video.Converters.Contracts;
using Uintra.Features.Media.Video.Converters.Implementations;
using Uintra.Features.Media.Video.Helpers.Contracts;
using Uintra.Features.Media.Video.Helpers.Implementations;
using Uintra.Features.Media.Video.Services.Contracts;
using Uintra.Features.Media.Video.Services.Implementations;
using Uintra.Features.Permissions;
using Uintra.Features.Permissions.Implementation;
using Uintra.Features.Permissions.Interfaces;
using Uintra.Features.Permissions.TypeProviders;
using Uintra.Features.Subscribe;
using Uintra.Infrastructure.ApplicationSettings;
using Uintra.Infrastructure.Caching;
using Uintra.Infrastructure.Context;
using Uintra.Infrastructure.Providers;
using Uintra.Infrastructure.TypeProviders;
using Uintra.Infrastructure.Utils;

namespace Uintra.Infrastructure.Ioc
{
    public class UintraInjectModule : IInjectModule
    {
        public IDependencyCollection Register(IDependencyCollection services)
        {
            //configurations
            services.AddSingleton<IApplicationSettings, ApplicationSettings.ApplicationSettings>();

            //services
            services.AddSingleton<IInformationService, InformationService>();
            services.AddSingleton<IDocumentTypeAliasProvider, DocumentTypeProvider>();
            services.AddSingleton<IIntranetMemberGroupService, IntranetMemberGroupService>();
            services.AddSingleton<IPermissionSettingsSchemaProvider, PermissionSettingsSchemaProvider>();
            services.AddSingleton<IContentPageContentProvider, ContentPageContentProvider>();
            services.AddSingleton(i =>
                (ILocalizationConfigurationSection) ConfigurationManager.GetSection("localizationConfiguration"));

            services.AddScoped<ICacheService, MemoryCacheService>();
            services.AddScoped<IEmbeddedResourceService, EmbeddedResourceService>();
            services.AddScoped<IMediaHelper, MediaHelper>();
            services.AddScoped<IMediaFolderTypeProvider>(provider => new MediaFolderTypeProvider(typeof(MediaFolderTypeEnum)));
            services.AddScoped<IImageHelper, ImageHelper>();
            services.AddScoped<IVideoConverter, VideoConverter>();
            services.AddScoped<IIntranetMediaService, IntranetMediaService>();
            services.AddScoped<IIntranetMemberGroupProvider, IntranetMemberGroupProvider>();
            services.AddScoped<IPermissionsService, PermissionsService>();
            services.AddScoped<IPermissionActionTypeProvider>(provider =>
                new PermissionActionTypeProvider(typeof(PermissionActionEnum)));
            services.AddScoped<IPermissionResourceTypeProvider>(provider =>
                new PermissionActivityTypeProvider(typeof(PermissionResourceTypeEnum)));
            services.AddScoped<IDateTimeFormatProvider, DateTimeFormatProvider>();
            services.AddScoped<IClientTimezoneProvider, ClientTimezoneProvider>();
            services.AddScoped<ICookieProvider, CookieProvider>();
            services.AddScoped<ISubscribeService, SubscribeService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IIntranetLocalizationService, LocalizationService>();
            services.AddScoped<ILocalizationCoreService, LocalizationCoreService>();
            services.AddScoped<ILocalizationStorageService, LocalizationStorageService>();
            services.AddScoped<ILocalizationCacheProvider, LocalizationMemoryCacheProvider>();
            services.AddScoped<ILocalizationCacheService, LocalizationCacheService>();
            services.AddScoped<ILocalizationSettingsService, LocalizationSettingsService>();
            services.AddScoped<ILocalizationResourceCacheService, LocalizationResourceCacheService>();
            services.AddScoped<ILightboxHelper, LightboxHelper>();
            services.AddScoped<IUBaselineRequestContext, IntranetRequestContext>();

            services.AddTransient<IVideoHelper, VideoHelper>();
            services.AddTransient<IVideoConverterLogService, VideoConverterLogService>();

            services.AddScoped<IMigrationHistoryService, MigrationHistoryService>();
            services.AddScoped<IMigration, Migration>();

            return services;
        }
    }
}