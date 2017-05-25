using Newtonsoft.Json;

namespace uIntra.Navigation.Core.Dashboard
{
    public class CreateNavigationCompositionsModel
    {
        [JsonProperty("parentIdOrAlias")]
        public string ParentIdOrAlias { get; set; }
    }
}