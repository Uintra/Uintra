using System;
using uIntra.Core.Activity;
using uIntra.Core.User;

namespace uIntra.News
{
    public interface INewsBase : IIntranetActivity
    {
        int? UmbracoCreatorId { get; set; }
        Guid CreatorId { get; set; }
        DateTime PublishDate { get; set; }
        DateTime? UnpublishDate { get; set; }
    }

    public class NewsBase : IntranetActivity, IHaveCreator, INewsBase
    {
        public int? UmbracoCreatorId { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime? UnpublishDate { get; set; }
    }
}