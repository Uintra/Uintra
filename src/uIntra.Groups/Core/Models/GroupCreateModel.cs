using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Uintra.Core.Media;

namespace Uintra.Groups
{
    public class GroupCreateModel : IContentWithMediaCreateEditModel
    {
        public GroupMemberSubscriptionModel Creator { get; set; }

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