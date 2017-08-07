using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using uIntra.Core.Persistence;

namespace uIntra.Navigation
{
    [uIntraTable("MyLink")]
    public class MyLink : SqlEntity<Guid>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public int ContentId { get; set; }

        public string QueryString { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid? ActivityId { get; set; }

    }
}