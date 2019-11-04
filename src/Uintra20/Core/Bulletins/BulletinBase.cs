using System;
using Newtonsoft.Json;
using Uintra20.Core.Activity.Entities;
using Uintra20.Core.LinkPreview;
using Uintra20.Core.User;

namespace Uintra20.Core.Bulletins
{
    public class BulletinBase : IntranetActivity, IHaveCreator, IHaveOwner, IBulletinBase, IHasLinkPreview
    {
        public int? UmbracoCreatorId { get; set; }
        public Guid CreatorId { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime PublishDate { get; set; }

        [JsonIgnore]
        public LinkPreview.LinkPreview LinkPreview { get; set; }

        public int? LinkPreviewId { get; set; }
    }
}