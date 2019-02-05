using System;

namespace Uintra.Core.Permissions.Models
{
    public class EntityGroupPermissions
    {
        public Guid EntityId { get; }
        public IntranetMemberGroup Group { get; }
        public Enum[] Actions { get; }

        public EntityGroupPermissions(Guid entityId, IntranetMemberGroup group, Enum[] actions)
        {
            Group = group;
            EntityId = entityId;
            Actions = actions;
        }
    }
}