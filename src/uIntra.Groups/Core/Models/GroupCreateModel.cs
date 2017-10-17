using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using uIntra.Core.Media;

namespace uIntra.Groups
{
    public class GroupCreateModel : IContentWithMediaCreateEditModel
    {
        public Guid CreatorId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required, AllowHtml]
        public string Description { get; set; }

        public string AllowedMediaExtensions { get; set; }

        public string Media { get; set; }

        public int? MediaRootId { get; set; }

        public string NewMedia { get; set; }
    }
}