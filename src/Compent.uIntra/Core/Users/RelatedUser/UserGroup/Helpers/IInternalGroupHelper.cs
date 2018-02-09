using Umbraco.Core.Models.Membership;

namespace Compent.Uintra.Core.Users.RelatedUser.UserGroup.Helpers
{
    public interface IInternalGroupHelper
    {
        string GetGroupAlias(IUser user);
        string GetGroupName(IUser user);
    }
}
