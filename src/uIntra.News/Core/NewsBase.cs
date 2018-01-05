using System;
using Newtonsoft.Json;
using uIntra.Core.Activity;
using uIntra.Core.Location;
using uIntra.Core.User;

namespace uIntra.News
{
    public interface INewsBase : IIntranetActivity
    {
        int? UmbracoCreatorId { get; set; }
        Guid CreatorId { get; set; }
        Guid OwnerId { get; set; }
        DateTime PublishDate { get; set; }
        DateTime? UnpublishDate { get; set; }
    }

    public class NewsBase : IntranetActivity, IHaveCreator, IHaveOwner, INewsBase, IHaveLocation
    {
        public int? UmbracoCreatorId { get; set; }
        public Guid CreatorId { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime? UnpublishDate { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public ActivityLocation Location { get; set; }
    }
}