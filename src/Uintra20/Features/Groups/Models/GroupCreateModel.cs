using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Uintra20.Features.Media;

namespace Uintra20.Features.Groups.Models
{
    public class GroupCreateModel : IContentWithMediaCreateEditModel
    {
        [Required]
        public string Title { get; set; }
        [Required, AllowHtml]
        public string Description { get; set; }
        public string AllowedMediaExtensions { get; set; }
        public string Media { get; set; }
        public string NewMedia { get; set; }
    }
}