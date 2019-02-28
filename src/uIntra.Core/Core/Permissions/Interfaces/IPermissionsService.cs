using System;
using System.Collections.Generic;
using Uintra.Core.Permissions.Models;
using Uintra.Core.User;

namespace Uintra.Core.Permissions.Interfaces
{
    public interface IPermissionsService
    {
        IEnumerable<PermissionModel> GetAll();
        IEnumerable<PermissionManagementModel> GetForGroup(IntranetMemberGroup group);
        PermissionModel Save(PermissionUpdateModel update);
        void DeletePermissionsForMemberGroup(int memberGroupId);
        bool Check(IIntranetMember member, PermissionSettingIdentity settingsIdentity);
        bool Check(PermissionSettingIdentity settingsIdentity);
        bool Check(Enum resourceType, Enum actionType);
    }
}
