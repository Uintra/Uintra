using System.Collections.Generic;
using LanguageExt;
using Uintra.Core.Permissions.Models;

namespace Uintra.Core.Permissions
{
    public interface IBasePermissionsService
    {
        IReadOnlyCollection<BasePermissionModel> GetAll();
        IEnumerable<BasePermissionModel> GetForGroup(IntranetMemberGroup group);
        BasePermissionModel Save(BasePermissionUpdateModel update);
        void DeletePermissionsForMemberGroup(int memberGroupId);
        bool Check(PermissionActivityTypeEnum permissionActivityType, PermissionActionEnum permissionAction);
        bool Check(int groupId, PermissionActivityTypeEnum permissionActivityType, PermissionActionEnum permissionAction);

    }
}
