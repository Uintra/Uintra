using System.Threading.Tasks;
using Uintra20.Features.Notification.Models;
using Uintra20.Features.Notification.Models.Configuration;
using Uintra20.Features.Notification.Models.NotifierTemplates;

namespace Uintra20.Features.Notification.Services
{
    public interface INotificationSettingsService
    {
        NotifierSettingsModel GetSettings(ActivityEventIdentity activityEventIdentity);
        NotifierSettingModel<T> Get<T>(ActivityEventNotifierIdentity activityEventNotifierIdentity) where T : INotifierTemplate;
        void Save<T>(NotifierSettingModel<T> settingModel) where T : INotifierTemplate;

        Task<NotifierSettingsModel> GetSettingsAsync(ActivityEventIdentity activityEventIdentity);
        Task<NotifierSettingModel<T>> GetAsync<T>(ActivityEventNotifierIdentity activityEventNotifierIdentity) where T : INotifierTemplate;
        Task SaveAsync<T>(NotifierSettingModel<T> settingModel) where T : INotifierTemplate;
    }
}