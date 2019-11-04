using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Uintra20.Persistence;

namespace Uintra20.Core.Groups.Sql
{
    [UintraTable(nameof(GroupActivityRelation))]
    public class GroupActivityRelation : SqlEntity<Guid>
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