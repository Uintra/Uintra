using Compent.Shared.DependencyInjection.Contract;
using Uintra.Core.Activity;
using Uintra.Core.Activity.Helpers;
using Uintra.Features.Groups.Services;
using Uintra.Features.Location.Services;
using Uintra.Features.Social;
using Uintra.Features.Social.Entities;
using Uintra.Features.Subscribe;
using Uintra.Features.Tagging.UserTags;
using Uintra.Infrastructure.TypeProviders;

namespace Uintra.Infrastructure.Ioc
{
    public class ActivityInjectModule: IInjectModule
	{
		public IDependencyCollection Register(IDependencyCollection services)
		{
            services.AddTransient<IGroupActivityService, GroupActivityService>();
            services.AddScoped<IActivityPageHelper, ActivityPageHelper>();
            services.AddTransient<IActivityTypeHelper, ActivityTypeHelper>();
            services.AddScoped<IIntranetActivityRepository, IntranetActivityRepository>();
            services.AddScoped<IActivityTypeProvider>(provider => new ActivityTypeProvider(typeof(IntranetActivityTypeEnum)));
            services.AddScoped<IActivitiesServiceFactory, ActivitiesServiceFactory>();
            services.AddTransient<IActivityLocationService, ActivityLocationService>();
            services.AddScoped<IActivityTagsHelper, ActivityTagsHelper>();
            services.AddScoped<IIntranetActivityService, SocialService<Social>>();
            services.AddScoped<IFeedActivityHelper, FeedActivityHelper>();
            services.AddScoped<IActivitySubscribeSettingService, ActivitySubscribeSettingService>();
            
            return services;
		}
	}
}