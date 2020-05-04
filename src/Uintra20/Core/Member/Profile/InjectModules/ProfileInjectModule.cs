using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Core.Member.Profile.Edit.Builders;

namespace Uintra20.Core.Member.Profile.InjectModules
{
    public class ProfileInjectModule : IInjectModule
    {
        public IDependencyCollection Register(IDependencyCollection services)
        {
            services.AddScoped<IProfileEditPageViewModelBuilder, ProfileEditPageViewModelBuilder>();

            return services;
        }
    }
}