using System;

namespace Uintra.Core.Permissions.Models
{
    public class TransientPermissionEntity
    {
        public Guid Id { get; set; }

        public Guid EntityId { get; set; }

        public Role RoleType { get; set; }

        public Enum PermissionType { get; set; }
    }
}