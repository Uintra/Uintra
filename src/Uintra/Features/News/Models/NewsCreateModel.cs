using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Uintra.Attributes;
using Uintra.Core.Activity.Models;
using Uintra.Features.Media;
using Uintra.Features.Media.Contracts;

namespace Uintra.Features.News.Models
{
    public class NewsCreateModel : 
        IntranetActivityCreateModelBase, 
        IContentWithMediaCreateEditModel
    {
        [Required, AllowHtml]
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime? UnpublishDate { get; set; }
        public string Media { get; set; }
        public string NewMedia { get; set; }
        [RequiredIf("IsPinned", true), GreaterThan("PublishDate")]
        public override DateTime? EndPinDate { get; set; }
        public bool PinAllowed { get; set; }
        public IEnumerable<Guid> TagIdsData { get; set; }
        public Guid? GroupId { get; set; }
    }
}