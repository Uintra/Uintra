using Uintra.Core.Member.Profile.Edit.Models;
using Uintra.Core.Member.Profile.Models;

namespace Uintra.Core.User
{
    public interface IIntranetUserContentProvider
    {
        ProfilePageModel GetProfilePage();
        ProfileEditPageModel GetEditPage();
    }
}
