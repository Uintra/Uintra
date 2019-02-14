
using Uintra.Core.Activity;
using Uintra.Core.User;
using Umbraco.Core.Models;

namespace Uintra.Groups.Permissions
{
    public interface IGroupPermissionsService
    {
        bool ValidatePermission(IPublishedContent content, IRole role);
        bool HasPermission(IRole role, IntranetActionEnum action);
    }
}