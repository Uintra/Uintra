using Uintra20.Core.Member.Profile.Edit.Models;
using Uintra20.Core.Member.Profile.Models;

namespace Uintra20.Core.User
{
    public interface IIntranetUserContentProvider
    {
        ProfilePageModel GetProfilePage();
        ProfileEditPageModel GetEditPage();
    }
}
