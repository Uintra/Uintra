using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Helpers;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Location.Services;

using Uintra20.Features.Tagging.UserTags;
using Uintra20.Infrastructure.TypeProviders;

namespace Uintra20.Infrastructure.Ioc
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
            
            return services;
		}
	}
}