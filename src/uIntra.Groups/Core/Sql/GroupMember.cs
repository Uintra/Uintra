using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Uintra.Core.Persistence;

namespace Uintra.Groups.Sql
{
    [UintraTable(nameof(GroupMember))]
    public class GroupMember : SqlEntity<Guid>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [Required, Index("UQ_GroupMember_GroupId_MemberId", 1, IsUnique = true)]
        public Guid GroupId { get; set; }

        [Required, Index("UQ_GroupMember_GroupId_MemberId", 2, IsUnique = true)]
        public Guid MemberId { get; set; }

        public bool IsAdmin { get; set; }
    }
}