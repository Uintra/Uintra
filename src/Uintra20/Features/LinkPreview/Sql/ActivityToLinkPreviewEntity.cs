using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Uintra20.Persistence;
using Uintra20.Persistence.Sql;

namespace Uintra20.Features.LinkPreview.Sql
{
    [UintraTable("ActivityToLinkPreview")]
    public class ActivityToLinkPreviewEntity : SqlEntity<int>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        [Required, Index("UQ_ActivityToLinkPreview_ActivityId_LinkPreviewId", 1, IsUnique = true)]
        public Guid ActivityId { get; set; }

        [Required, Index("UQ_ActivityToLinkPreview_ActivityId_LinkPreviewId", 2, IsUnique = true)]
        public int LinkPreviewId { get; set; }
    }
}