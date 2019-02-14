using System.Collections.Generic;
using LanguageExt;
using Uintra.Core.Permissions.Models;

namespace Uintra.Core.Permissions.Interfaces
{
    public interface IPermissionsManagementService
    {
        IEnumerable<PermissionManagementModel> GetGroupManagement(IntranetMemberGroup group);
        Unit Save(PermissionManagementModel update);
    }
}
