using Newtonsoft.Json;
using uCommunity.Core.User;

namespace uCommunity.News
{
    public class NewsModelBase : NewsBase, IHaveCreator
    {
        [JsonIgnore]
        public IIntranetUser Creator { get; set; }
    }
}
