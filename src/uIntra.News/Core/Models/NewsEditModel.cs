using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using uIntra.Core.Activity;
using uIntra.Core.Media;
using uIntra.Core.ModelBinders;
using uIntra.Core.User;

namespace uIntra.News
{
    public class NewsEditModel : IntranetActivityEditModelBase, IContentWithMediaCreateEditModel, ICanEditCreatorCreateEditModel
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

        [Required]
        public Guid CreatorId { get; set; }

        public IIntranetUser Creator { get; set; }

        public IEnumerable<IIntranetUser> Users { get; set; }

        public bool CanEditCreator { get; set; }

        public NewsEditModel()
        {
            Users = Enumerable.Empty<IIntranetUser>();
        }
    }
}