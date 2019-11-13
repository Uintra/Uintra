using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Uintra20.Persistence;

namespace Uintra20.Core.Groups.Sql
{
    [UintraTable(nameof(Group))]
    public class Group : SqlEntity<Guid>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public Guid CreatorId { get; set; }

        public int? ImageId { get; set; }

        public bool IsHidden { get; set; }

        public int GroupTypeId { get; set; }
    }
}