using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Uintra20.Persistence;
using Uintra20.Persistence.Sql;

namespace Uintra20.Features.Location.Sql
{
    [UintraTable("ActivityLocation")]
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