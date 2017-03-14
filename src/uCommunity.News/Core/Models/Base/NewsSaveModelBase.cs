using System;
using System.Web.Mvc;
using ServiceStack.DataAnnotations;
using uCommunity.Core.Activity.Models;
using uCommunity.Core.Media;

namespace uCommunity.News
{
    public class NewsSaveModelBase : IntranetActivitySaveModelBase, IContentWithMediaCreateEditModel
    {
        [Required, StringLength(4000)]
        public string Teaser { get; set; }

        [Required, AllowHtml]
        public string Description { get; set; }

        [Required]
        public DateTime PublishDate { get; set; }

        public string Media { get; set; }

        public int? MediaRootId { get; set; }

        public string NewMedia { get; set; }
    }
}