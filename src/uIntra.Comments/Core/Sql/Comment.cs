using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Uintra.Core.Persistence;

namespace Uintra.Comments
{
    [UintraTable("Comment")]
    public class Comment : SqlEntity<Guid>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid ActivityId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifyDate { get; set; }

        public string Text { get; set; }

        public Guid? ParentId { get; set; }
    }
}