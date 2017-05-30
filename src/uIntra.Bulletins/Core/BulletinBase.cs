using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using uIntra.Core.Activity;
using uIntra.Core.User;

namespace uIntra.Bulletins
{
    public class BulletinBase : IntranetActivity, IHaveCreator, IBulletinBase
    {
        public int? UmbracoCreatorId { get; set; }
        public Guid CreatorId { get; set; }

        [JsonIgnore]
        public IIntranetUser Creator { get; set; }
        public IEnumerable<int> MediaIds { get; set; } = Enumerable.Empty<int>();
        public DateTime PublishDate { get; set; }
    }
}