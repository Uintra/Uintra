using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Factories;
using Uintra20.Core.Activity.Helpers;
using Uintra20.Core.Member;
using Uintra20.Features.CentralFeed.Links;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Information;
using Uintra20.Features.Location.Services;
using Uintra20.Features.Media;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Implementation;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Permissions.TypeProviders;
using Uintra20.Features.Tagging.UserTags;
using Uintra20.Infrastructure.ApplicationSettings;
using Uintra20.Infrastructure.Caching;
using Uintra20.Infrastructure.Exceptions;
using Uintra20.Infrastructure.Providers;
using Uintra20.Infrastructure.TypeProviders;
using Uintra20.Infrastructure.Utils;
using Umbraco.Web;

namespace Uintra20.Infrastructure.Ioc
{
	public class ActivityInjectModule: IInjectModule
	{
		public IDependencyCollection Register(IDependencyCollection services)
		{
            services.AddTransient<IGroupActivityService, GroupActivityService>();
            services.AddTransient<IActivityPageHelperFactory>(provider =>
                new CacheActivityPageHelperFactory(provider.GetService<UmbracoHelper>(),
                    provider.GetService<IDocumentTypeAliasProvider>(),
                    CentralFeedLinkProviderHelper.GetFeedActivitiesXPath(provider.GetService<IDocumentTypeAliasProvider>())));
            services.AddTransient<IActivityTypeHelper, ActivityTypeHelper>();
            services.AddScoped<IIntranetActivityRepository, IntranetActivityRepository>();
            services.AddScoped<IActivityTypeProvider>(provider => new ActivityTypeProvider(typeof(IntranetActivityTypeEnum)));
            services.AddScoped<IActivitiesServiceFactory, ActivitiesServiceFactory>();
            services.AddTransient<IActivityLocationService, ActivityLocationService>();
            services.AddScoped<IActivityTagsHelper, ActivityTagsHelper>();

            return services;
		}
	}
}