using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Features.ContentPage.Services;
using Uintra20.Features.Notification.Services;

namespace Uintra20.Features.ContentPage.InjectModule
{
    public class ContentPageInjectModule : IInjectModule
    {
        public IDependencyCollection Register(IDependencyCollection services)
        {
            services.AddScoped<INotifyableService, ContentPageNotificationService>();

            return services;
        }
    }
}