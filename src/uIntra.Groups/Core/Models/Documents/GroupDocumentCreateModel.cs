using System;
using System.ComponentModel.DataAnnotations;
using Uintra.Core.Media;

namespace Uintra.Groups
{
    public class GroupDocumentCreateModel : IContentWithMediaCreateEditModel
    {
        [Required]
        public Guid GroupId { get; set; }
        public int? MediaRootId { get; set; }
        [Required]
        public string NewMedia { get; set; }
    }
}
