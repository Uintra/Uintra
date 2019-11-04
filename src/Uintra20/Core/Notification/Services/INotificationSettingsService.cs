using System.Threading.Tasks;

namespace Uintra20.Core.Notification
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