using System.Collections.Generic;
using Uintra.Core.Permissions.Models;
using Uintra.Core.User;

namespace Uintra.Core.Permissions
{
    public interface IBasePermissionsService
    {
        IEnumerable<BasePermissionModel> GetAll();
        IEnumerable<BasePermissionModel> GetForGroup(IntranetMemberGroup group);
        BasePermissionModel Save(BasePermissionUpdateModel update);
        void DeletePermissionsForMemberGroup(int memberGroupId);
        bool Check(IIntranetMember member, PermissionActivityTypeEnum permissionActivityType, PermissionActionEnum permissionAction);
        bool Check(PermissionActivityTypeEnum permissionActivityType, PermissionActionEnum permissionAction);
    }
}
