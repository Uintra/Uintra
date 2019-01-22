using System;

namespace Uintra.Core.Permissions.Models
{
    public class EntityRolePermissions
    {
        public Guid EntityId { get; }
        public Role Role { get; }
        public Enum[] Permissions { get; }

        public EntityRolePermissions(Guid entityId, Role role, Enum[] permissions)
        {
            Role = role;
            EntityId = entityId;
            Permissions = permissions;
        }
    }
}