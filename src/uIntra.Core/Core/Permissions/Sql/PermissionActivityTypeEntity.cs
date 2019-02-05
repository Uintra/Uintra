using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Uintra.Core.Persistence;

namespace Uintra.Core.Permissions.Sql
{
    [UintraTable("PermissionActivityType")]
    public class PermissionActivityTypeEntity : SqlEntity<Guid>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public string Activity { get; set; }        
    }
}
