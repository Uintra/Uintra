using System.Threading.Tasks;
using Uintra.Core.Member.Profile.Edit.Models;

namespace Uintra.Core.Member.Profile.Services
{
    public interface IProfileService
    {
        Task Delete(int photoId);
        Task UpdateNotificationSettings(ProfileEditNotificationSettings settings);
        Task Edit(ProfileEditModel editModel);
        Task<ProfileEditViewModel> GetCurrentUserProfile();
    }
}