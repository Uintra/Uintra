using Newtonsoft.Json;

namespace Uintra.Users.UserList
{
    public class DetailsPopupModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
