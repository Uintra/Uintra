using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using uIntra.Core.Activity.Entities;
using uIntra.Core.User;

namespace uCommunity.News
{
    public interface INewsBase : IIntranetActivity
    {
        int? UmbracoCreatorId { get; set; }
        Guid CreatorId { get; set; }
        IIntranetUser Creator { get; set; }
        IEnumerable<int> MediaIds { get; set; }
        DateTime PublishDate { get; set; }
        DateTime? UnpublishDate { get; set; }
    }

    public class NewsBase : IntranetActivity, IHaveCreator, INewsBase
    {
        public int? UmbracoCreatorId { get; set; }
        public Guid CreatorId { get; set; }
        [JsonIgnore]
        public IIntranetUser Creator { get; set; }
        public IEnumerable<int> MediaIds { get; set; } = Enumerable.Empty<int>();
        public DateTime PublishDate { get; set; }
        public DateTime? UnpublishDate { get; set; }
    }
}