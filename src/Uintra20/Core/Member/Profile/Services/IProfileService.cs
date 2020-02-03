using System.Threading.Tasks;
using Uintra20.Core.Member.Profile.Edit.Models;

namespace Uintra20.Core.Member.Profile.Services
{
    public interface IProfileService
    {
        Task Delete(int photoId);
        Task UpdateNotificationSettings(ProfileEditNotificationSettings settings);
        Task Edit(ProfileEditModel model);
        Task<ProfileEditModel> GetCurrentUserProfile();
    }
}