using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using uCommunity.Core.Activity.Models;
using uCommunity.Core.Media;
using uCommunity.Core.ModelBinders;

namespace uCommunity.News
{
    public class NewsCreateModel : IntranetActivityCreateModelBase, IContentWithMediaCreateEditModel
    {
        [Required, AllowHtml]
        public string Description { get; set; }

        [PropertyBinder(typeof(DateTimeBinder))]
        public DateTime PublishDate { get; set; }

        [PropertyBinder(typeof(DateTimeBinder))]
        public DateTime? UnpublishDate { get; set; }

        public string Media { get; set; }

        public int? MediaRootId { get; set; }

        public string NewMedia { get; set; }

        public Guid CreatorId { get; set; }
    }
}