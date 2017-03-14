using Newtonsoft.Json;

namespace uCommunity.Navigation.Core.Dashboard
{
    public class NavigationInitialState
    {
        [JsonProperty("isDocumentTypesAlreadyExists")]
        public bool IsDocumentTypesAlreadyExists { get; set; }
    }
}