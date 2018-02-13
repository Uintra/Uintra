using System;
using Uintra.Core.Activity;
using Uintra.Core.User;

namespace Uintra.Core.PagePromotion
{
    public class PagePromotionBase : IntranetActivity, IHaveCreator, IHaveOwner
    {
        public string Url { get; set; }

        public string PageAlias { get; set; }

        public DateTime PublishDate { get; set; }

        public Guid CreatorId { get; set; }

        public int? UmbracoCreatorId { get; set; }

        public Guid OwnerId
        {
            get => CreatorId;
            set { }
        }
    }
}
