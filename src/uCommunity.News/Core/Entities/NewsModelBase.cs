using System;
using Newtonsoft.Json;
using uCommunity.Core.User;

namespace uCommunity.News
{
    public class NewsModelBase : NewsBase, IHasCreator<IntranetUserBase>
    {
        [JsonIgnore]
        public DateTime SortDate => PublishDate;
        [JsonIgnore]
        public IntranetUserBase Creator { get; set; }
    }
}
