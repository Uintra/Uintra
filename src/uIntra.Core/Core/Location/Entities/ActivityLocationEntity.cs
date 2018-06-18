using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using uIntra.Core.Activity;
using uIntra.Core.Persistence;

namespace uIntra.Core.Location.Entities
{
    [uIntraTable("ActivityLocation")]
    public class ActivityLocationEntity : SqlEntity<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        [Index("UQ_ActivityLocation_ActivityId", IsUnique = true)]
        public Guid ActivityId { get; set; }

        public string Address { get; set; }
        public string ShortAddress { get; set; }
    }
}
