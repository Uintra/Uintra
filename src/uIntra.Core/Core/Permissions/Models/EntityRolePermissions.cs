using System;

namespace Uintra.Core.Permissions.Models
{
    public class EntityGroupPermissions
    {
        public Guid EntityId { get; }
        public IntranetMemberGroup Group { get; }
        public Enum[] Permissions { get; }

        public EntityGroupPermissions(Guid entityId, IntranetMemberGroup group, Enum[] permissions)
        {
            Group = group;
            EntityId = entityId;
            Permissions = permissions;
        }
    }
}