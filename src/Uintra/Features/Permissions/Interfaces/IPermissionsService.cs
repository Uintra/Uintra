﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uintra.Core.Member.Abstractions;
using Uintra.Features.Permissions.Models;

namespace Uintra.Features.Permissions.Interfaces
{
    public interface IPermissionsService
    {
        void Save(IEnumerable<PermissionUpdateModel> permissions);
        void DeletePermissionsForMemberGroup(int memberGroupId);
        bool Check(IIntranetMember member, PermissionSettingIdentity settingsIdentity);
        bool Check(PermissionSettingIdentity settingsIdentity);
        bool Check(Enum resourceType, Enum actionType);
        PermissionModel Save(PermissionUpdateModel update);
        IEnumerable<PermissionModel> GetAll();
        IEnumerable<PermissionManagementModel> GetForGroup(IntranetMemberGroup group);
        Task<bool> ContainsDefaultPermissionsAsync();
        Task<bool> CheckAsync(IIntranetMember member, PermissionSettingIdentity settingsIdentity);
        Task<bool> CheckAsync(PermissionSettingIdentity settingsIdentity);
        Task<bool> CheckAsync(Enum resourceType, Enum actionType);
        Task<IEnumerable<PermissionModel>> GetAllAsync();
        Task<IEnumerable<PermissionManagementModel>> GetForGroupAsync(IntranetMemberGroup group);
        Task<PermissionModel> SaveAsync(PermissionUpdateModel update);
        Task SaveAsync(IEnumerable<PermissionUpdateModel> permissions);
        Task DeletePermissionsForMemberGroupAsync(int memberGroupId);
    }
}
