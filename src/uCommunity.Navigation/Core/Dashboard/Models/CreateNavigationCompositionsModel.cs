using Newtonsoft.Json;

namespace uCommunity.Navigation.Core.Dashboard
{
    public class CreateNavigationCompositionsModel
    {
        [JsonProperty("folderId")]
        public string FolderId { get; set; }
    }
}