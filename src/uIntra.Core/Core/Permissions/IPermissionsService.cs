using System;
using LanguageExt;
using Uintra.Core.Permissions.Models;

namespace Uintra.Core.Permissions
{
    public interface IPermissionsService
    {
        EntityPermissions Get(Guid entityId);
        EntityRolePermissions GeForRole(Guid entityId, Role role);
        bool Has(Guid entityId, Role role, Enum[] permissions);
        Unit Save(EntityPermissions permissions);
        Unit Save(EntityRolePermissions permissions);
        Unit Add(Guid entityId, Role role, Enum[] permissions);
        Unit Remove(Guid entityId, Role role, Enum[] permissions);
    }
}
