using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Entities;
using Uintra20.Core.Activity.Factories;
using Uintra20.Core.Activity.Helpers;
using Uintra20.Features.Bulletins;
using Uintra20.Features.Bulletins.Entities;
using Uintra20.Features.CentralFeed.Links;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Location.Services;
using Uintra20.Features.Tagging.UserTags;
using Uintra20.Infrastructure.Providers;
using Uintra20.Infrastructure.TypeProviders;
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
                    provider.GetService<IDocumentTypeAliasProvider>()));//,
                    //CentralFeedLinkProviderHelper.GetFeedActivitiesXPath(provider.GetService<IDocumentTypeAliasProvider>())));
            services.AddTransient<IActivityTypeHelper, ActivityTypeHelper>();
            services.AddScoped<IIntranetActivityRepository, IntranetActivityRepository>();
            services.AddScoped<IActivityTypeProvider>(provider => new ActivityTypeProvider(typeof(IntranetActivityTypeEnum)));
            services.AddScoped<IActivitiesServiceFactory, ActivitiesServiceFactory>();
            services.AddTransient<IActivityLocationService, ActivityLocationService>();
            services.AddScoped<IActivityTagsHelper, ActivityTagsHelper>(); 
            //services.AddScoped<IIntranetActivityService<Bulletin>, BulletinsService<Bulletin>>();
            services.AddScoped<IIntranetActivityService, BulletinsService<Bulletin>>();

            return services;
		}
	}
}