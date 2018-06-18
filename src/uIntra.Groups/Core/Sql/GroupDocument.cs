using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using uIntra.Core.Persistence;

namespace uIntra.Groups.Sql
{
    [uIntraTable(nameof(GroupDocument))]
    public class GroupDocument : SqlEntity<Guid>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public Guid GroupId { get; set; }

        public int MediaId { get; set; }
    }
}