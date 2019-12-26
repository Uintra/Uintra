using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Core.Authentication;
using Uintra20.Core.Controls.LightboxGallery;
using Uintra20.Core.Localization;
using Uintra20.Core.Member;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Information;
using Uintra20.Features.Media;
using Uintra20.Features.News;
using Uintra20.Features.News.Entities;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Implementation;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Permissions.TypeProviders;
using Uintra20.Features.Subscribe;
using Uintra20.Infrastructure.ApplicationSettings;
using Uintra20.Infrastructure.Caching;
using Uintra20.Infrastructure.Exceptions;
using Uintra20.Infrastructure.Providers;
using Uintra20.Infrastructure.TypeProviders;
using Uintra20.Infrastructure.Utils;

namespace Uintra20.Infrastructure.Ioc
{
	public class UintraInjectModule: IInjectModule
	{
		public IDependencyCollection Register(IDependencyCollection services)
		{
			//configurations
			services.AddSingleton<IApplicationSettings, ApplicationSettings.ApplicationSettings>();

			//services
			services.AddSingleton<IInformationService,InformationService>();

            services.AddScoped<ICacheService, MemoryCacheService>();
            services.AddSingleton<IIntranetMemberGroupService, IntranetMemberGroupService>();
            services.AddScoped<IEmbeddedResourceService, EmbeddedResourceService>();
            services.AddScoped<IExceptionLogger, ExceptionLogger>();
            services.AddScoped<IMediaHelper, MediaHelper>();
            services.AddScoped<IMediaFolderTypeProvider>(provider => new MediaFolderTypeProvider(typeof(MediaFolderTypeEnum)));
            services.AddScoped<IImageHelper, ImageHelper>();
            services.AddScoped<IVideoHelper, VideoHelper>();
            services.AddScoped<IVideoConverter, VideoConverter>();
            services.AddScoped<IVideoConverterLogService, VideoConverterLogService>();
            services.AddScoped<IIntranetMediaService, IntranetMediaService>();
            services.AddSingleton<IDocumentTypeAliasProvider, DocumentTypeProvider>();
            //services.AddScoped<IXPathProvider, XPathProvider>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddSingleton<IIntranetMemberGroupService, IntranetMemberGroupService>();
            services.AddSingleton<IPermissionSettingsSchemaProvider, PermissionSettingsSchemaProvider>();
            services.AddScoped<IPermissionsService, PermissionsService>();
            services.AddScoped<IPermissionActionTypeProvider>(provider => new PermissionActionTypeProvider(typeof(PermissionActionEnum)));
            services.AddScoped<IPermissionResourceTypeProvider>(provider => new PermissionActivityTypeProvider(typeof(PermissionResourceTypeEnum)));
            services.AddScoped<IIntranetMemberGroupProvider, IntranetMemberGroupProvider>();
            services.AddScoped<IDateTimeFormatProvider, DateTimeFormatProvider>();
            services.AddScoped<IClientTimezoneProvider, ClientTimezoneProvider>();
            services.AddScoped<ICookieProvider, CookieProvider>();
            services.AddScoped<IMentionService, MentionService>();
            services.AddScoped<ISubscribeService, SubscribeService>();
            services.AddScoped<Uintra20.Core.Authentication.IAuthenticationService, Uintra20.Core.Authentication.AuthenticationService>();
            services.AddScoped<IIntranetLocalizationService, LocalizationService>();

            services.AddScoped<INewsService<News>, NewsService>();

            services.AddScoped<ILightboxHelper, LightboxHelper>();

			return services;
		}
	}
}