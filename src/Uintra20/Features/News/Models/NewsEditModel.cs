using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Uintra20.Attributes;
using Uintra20.Core.Activity.Models;
using Uintra20.Features.Media;

namespace Uintra20.Features.News.Models
{
    public class NewsEditModel : 
        IntranetActivityEditModelBase, 
        IContentWithMediaCreateEditModel
    {
        [Required, AllowHtml]
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime? UnpublishDate { get; set; }
        public string Media { get; set; }
        public int? MediaRootId { get; set; }
        public string NewMedia { get; set; }
        [RequiredIf("IsPinned", true), GreaterThan("PublishDate")]
        public override DateTime? EndPinDate { get; set; }
        public bool PinAllowed { get; set; }
        public string TagIdsData { get; set; }
    }
}