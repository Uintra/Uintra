using Newtonsoft.Json;
using uCommunity.Core.User;

namespace uCommunity.News
{
    public class NewsModelBase : NewsBase, IHasCreator<IntranetUserBase>
    {
        [JsonIgnore]
        public IntranetUserBase Creator { get; set; }
    }
}
