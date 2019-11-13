using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Uintra20.Persistence;

namespace Uintra20.Core.Media.Sql
{
    [UintraTable("Media")]
    public class IntranetMediaEntity : SqlEntity<Guid>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public Guid EntityId { get; set; }

        public string MediaIds { get; set; } = string.Empty;
    }
}