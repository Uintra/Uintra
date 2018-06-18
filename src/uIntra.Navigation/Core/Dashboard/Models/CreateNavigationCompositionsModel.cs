using Newtonsoft.Json;

namespace uIntra.Navigation.Dashboard
{
    public class CreateNavigationCompositionsModel
    {
        [JsonProperty("parentIdOrAlias")]
        public string ParentIdOrAlias { get; set; }
    }
}