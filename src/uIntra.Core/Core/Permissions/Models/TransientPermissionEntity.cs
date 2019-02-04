using System;

namespace Uintra.Core.Permissions.Models
{
    public class TransientPermissionEntity
    {
        public Guid Id { get; set; }

        public Guid EntityId { get; set; }

        public IntranetMemberGroup Group { get; set; }

        public Enum PermissionType { get; set; }
    }
}