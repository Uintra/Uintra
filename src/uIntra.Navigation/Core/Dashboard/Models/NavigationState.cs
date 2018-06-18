using Newtonsoft.Json;

namespace Uintra.Navigation.Dashboard
{
    public class NavigationState
    {
        [JsonProperty("isDocumentTypesAlreadyExists")]
        public bool IsDocumentTypesAlreadyExists { get; set; }

        [JsonProperty("isUnknownParent")]
        public bool IsUnknownParent { get; set; }
    }
}