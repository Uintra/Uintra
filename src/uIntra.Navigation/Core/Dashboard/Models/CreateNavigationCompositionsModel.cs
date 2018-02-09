using Newtonsoft.Json;

namespace Uintra.Navigation.Dashboard
{
    public class CreateNavigationCompositionsModel
    {
        [JsonProperty("parentIdOrAlias")]
        public string ParentIdOrAlias { get; set; }
    }
}