using System;
using System.Collections.Generic;

namespace Uintra.Core.Permissions.Models
{
    public class EntityPermissions
    {
        public Guid EntityId { get; }
        public Dictionary<IntranetMemberGroup, Enum[]> Permissions { get; }

        public EntityPermissions(Guid entityId, Dictionary<IntranetMemberGroup, Enum[]> permissions)
        {
            EntityId = entityId;
            Permissions = permissions;
        }
    }
}