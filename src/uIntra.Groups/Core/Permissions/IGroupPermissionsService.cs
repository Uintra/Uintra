
using uIntra.Core.Activity;
using uIntra.Core.User;
using Umbraco.Core.Models;

namespace uIntra.Groups.Permissions
{
    public interface IGroupPermissionsService
    {
        bool ValidatePermission(IPublishedContent content, IRole role);
        bool HasPermission(IRole role, IntranetActivityActionEnum action);
    }
}