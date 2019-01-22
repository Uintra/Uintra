using System;
using System.Collections.Generic;

namespace Uintra.Core.Permissions.Models
{
    public class EntityPermissions
    {
        public Guid EntityId { get; }
        public Dictionary<Role, Enum[]> Permissions { get; }

        public EntityPermissions(Guid entityId, Dictionary<Role, Enum[]> permissions)
        {
            EntityId = entityId;
            Permissions = permissions;
        }
    }
}