using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using uIntra.Core.Persistence;

namespace uIntra.Tagging
{
    [uIntraTable("TagActivityRelation")]
    public class TagActivityRelation : SqlEntity<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        [Required]
        [Index("UQ_TagActivityRelation_TagId_ActivityId", 1, IsUnique = true)]
        public Guid TagId { get; set; }

        [Required]
        [Index("UQ_TagActivityRelation_TagId_ActivityId", 2, IsUnique = true)]
        public Guid ActivityId { get; set; }
    }
}
