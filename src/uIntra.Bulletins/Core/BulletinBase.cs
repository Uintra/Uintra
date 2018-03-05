using System;
using Newtonsoft.Json;
using Uintra.Core.Activity;
using Uintra.Core.LinkPreview;
using Uintra.Core.User;

namespace Uintra.Bulletins
{
    public class BulletinBase : IntranetActivity, IHaveCreator, IHaveOwner, IBulletinBase, IHasLinkPreview
    {
        public int? UmbracoCreatorId { get; set; }
        public Guid CreatorId { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime PublishDate { get; set; }

        [JsonIgnore]
        public LinkPreview LinkPreview { get; set; }

        public int? LinkPreviewId { get; set; }
    }
}