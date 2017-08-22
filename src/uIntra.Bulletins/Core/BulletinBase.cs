using System;
using uIntra.Core.Activity;
using uIntra.Core.User;

namespace uIntra.Bulletins
{
    public class BulletinBase : IntranetActivity, IHaveCreator, IBulletinBase
    {
        public int? UmbracoCreatorId { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime PublishDate { get; set; }
    }
}