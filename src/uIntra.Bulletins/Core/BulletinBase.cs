using System;
using Uintra.Core.Activity;
using Uintra.Core.User;

namespace Uintra.Bulletins
{
    public class BulletinBase : IntranetActivity, IHaveCreator, IHaveOwner, IBulletinBase
    {
        public int? UmbracoCreatorId { get; set; }
        public Guid CreatorId { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime PublishDate { get; set; }
    }
}