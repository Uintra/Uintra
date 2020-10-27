using System;
using System.ComponentModel.DataAnnotations;
using Uintra.Features.Media;
using Uintra.Features.Media.Contracts;

namespace Uintra.Features.Groups.Models
{
    public class GroupDocumentCreateModel : IContentWithMediaCreateEditModel
    {
        [Required]
        public Guid GroupId { get; set; }
        [Required]
        public string NewMedia { get; set; }
    }
}