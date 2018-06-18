using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using uIntra.Core.Persistence;

namespace uIntra.Groups.Sql
{
    [uIntraTable(nameof(GroupMember))]
    public class GroupMember : SqlEntity<Guid>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [Required, Index("UQ_GroupMember_GroupId_MemberId", 1, IsUnique = true)]
        public Guid GroupId { get; set; }

        [Required, Index("UQ_GroupMember_GroupId_MemberId", 2, IsUnique = true)]
        public Guid MemberId { get; set; } 
    }
}