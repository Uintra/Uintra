using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using uIntra.Core.Persistence;

namespace uIntra.Tagging.UserTags
{
    [uIntraTable(nameof(UserTagRelation))]
    public class UserTagRelation : SqlEntity<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        [Required, Index("UQ_UserTagRelation_UserTagId_EntityId", 1, IsUnique = true)]
        public Guid UserTagId { get; set; }

        [Required, Index("UQ_UserTagRelation_UserTagId_EntityId", 2, IsUnique = true)]
        public Guid EntityId { get; set; }
    }
}