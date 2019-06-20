using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Uintra.Core.Persistence;

namespace Uintra.Core.Permissions.Sql
{
    [UintraTable("Permission")]
    public class PermissionEntity : SqlEntity<Guid>
    {
        [NotMapped]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }        

        [Key, Column(Order = 1), Index("UniqIndex", 1, IsUnique = true)]
        public int IntranetMemberGroupId { get; set; }

        [Key, Column(Order = 2), Index("UniqIndex", 2, IsUnique = true)]
        public int ActionId { get; set; }

        [Key, Column(Order = 3), Index("UniqIndex", 3, IsUnique = true)]
        public int ResourceTypeId { get; set; }

        public bool IsAllowed { get; set; }

        public bool IsEnabled { get; set; }
    }
}
