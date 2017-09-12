using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using uIntra.Core.Persistence;

namespace uIntra.Groups.Sql
{
    [uIntraTable(nameof(GroupActivity))]
    public class GroupActivity : SqlEntity<Guid>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [Required, Index("UQ_GroupActivity_GroupId_ActivityId", 1, IsUnique = true)]
        public Guid GroupId { get; set; }

        [Required, Index("UQ_GroupActivity_GroupId_ActivityId", 2, IsUnique = true)]
        public Guid ActivityId { get; set; } 
    }
}
