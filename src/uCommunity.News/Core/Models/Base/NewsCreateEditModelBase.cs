using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using uCommunity.Core.Activity.Models;
using uCommunity.Core.Media;
using uCommunity.Core.ModelBinders;

namespace uCommunity.News
{
    public class NewsCreateEditModelBase : IntranetActivityCreateModelBase, IContentWithMediaCreateEditModel
    {
        [Required, StringLength(4000)]
        public string Teaser { get; set; }

        [Required, AllowHtml]
        public string Description { get; set; }

        [PropertyBinder(typeof(DateTimeBinder))]
        public DateTime PublishDate { get; set; }

        public string Media { get; set; }

        public int? MediaRootId { get; set; }

        public string NewMedia { get; set; }
    }
}