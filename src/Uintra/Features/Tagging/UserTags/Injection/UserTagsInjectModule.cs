using Compent.Shared.DependencyInjection.Contract;
using Uintra.Features.Tagging.UserTags.Services;

namespace Uintra.Features.Tagging.UserTags.Injection
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