using System.Collections.Generic;
using Uintra.Core.Permissions.Models;
using Uintra.Core.User;

namespace Uintra.Core.Permissions
{
    public interface IPermissionsService
    {
        IEnumerable<PermissionModel> GetAll();
        IEnumerable<PermissionModel> GetForGroup(IntranetMemberGroup group);
        PermissionModel Save(PermissionUpdateModel update);
        void DeletePermissionsForMemberGroup(int memberGroupId);
        bool Check(IIntranetMember member, PermissionActivityTypeEnum permissionActivityType, PermissionActionEnum permissionAction);
        bool Check(PermissionActivityTypeEnum permissionActivityType, PermissionActionEnum permissionAction);
    }
}
