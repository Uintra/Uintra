using Compent.Shared.DependencyInjection.Contract;
using Uintra.Features.ContentPage.Services;
using Uintra.Features.Notification.Services;

namespace Uintra.Features.ContentPage.InjectModule
{
    public class ContentPageInjectModule : IInjectModule
    {
        public IDependencyCollection Register(IDependencyCollection services)
        {
            services.AddScopedToCollection<INotifyableService, ContentPageNotificationService>();

            return services;
        }
    }
}