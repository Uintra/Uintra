using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Features.Tagging.UserTags.Services;

namespace Uintra20.Features.Tagging.UserTags.Injection
{
    public class UserTagsInjectModule : IInjectModule
    {
        public IDependencyCollection Register(IDependencyCollection services)
        {
            //services.AddScoped<IUserTagProvider, UserTagProvider>();
            services.AddScoped<IUserTagRelationService, UserTagRelationService>();
           // services.AddScoped<IUserTagService, UserTagService>();

            return services;
        }
    }
}