using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using uIntra.Core.Persistence;

namespace uIntra.Core.LinkPreview.Sql
{
    [uIntraTable("LinkPreview")]
    public class LinkPreviewEntity : SqlEntity<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        [Required]
        public string Uri { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string OgDescription { get; set; }
        public Guid ImageId { get; set; }
        public Guid FaviconId { get; set; }
    }
}
