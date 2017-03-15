using Newtonsoft.Json;

namespace uCommunity.Navigation.Core.Dashboard
{
    public class CreateNavigationCompositionsModel
    {
        [JsonProperty("parentIdOrAlias")]
        public string ParentIdOrAlias { get; set; }
    }
}