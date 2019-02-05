using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Uintra.Core.Persistence;

namespace Uintra.Core.Permissions.Sql
{
    [UintraTable("Permission")]
    public class PermissionActivityTypeTargetEntity : SqlEntity<Guid>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public int ActivityTypeId { get; set; }
    }
}
