using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Uintra.Core.Persistence;

namespace Uintra.Core.Permissions.Sql
{
    [UintraTable("Permission")]
    public class PermissionEntity : SqlEntity<Guid>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }        

        public int IntranetMemberGroupId { get; set; }

        public int ActionId { get; set; }

        public int ResourceTypeId { get; set; }

        public bool IsAllowed { get; set; }

        public bool IsEnabled { get; set; }
    }
}
