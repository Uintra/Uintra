using System;
using System.Collections.Generic;
using Uintra20.Core.Permissions.Models;
using Uintra20.Core.User;

namespace Uintra20.Core.Permissions.Interfaces
{
    public interface IPermissionsService
    {
        IEnumerable<PermissionModel> GetAll();
        IEnumerable<PermissionManagementModel> GetForGroup(IntranetMemberGroup group);
        PermissionModel Save(PermissionUpdateModel update);
        void Save(IEnumerable<PermissionUpdateModel> permissions);
        void DeletePermissionsForMemberGroup(int memberGroupId);
        bool Check(IIntranetMember member, PermissionSettingIdentity settingsIdentity);
        bool Check(PermissionSettingIdentity settingsIdentity);
        bool Check(Enum resourceType, Enum actionType);
    }
}
