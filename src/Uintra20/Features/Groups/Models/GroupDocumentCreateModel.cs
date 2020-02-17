using System;
using System.ComponentModel.DataAnnotations;
using Uintra20.Features.Media;

namespace Uintra20.Features.Groups.Models
{
    public class GroupDocumentCreateModel : IContentWithMediaCreateEditModel
    {
        [Required]
        public Guid GroupId { get; set; }
        [Required]
        public string NewMedia { get; set; }
    }
}