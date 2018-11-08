using Newtonsoft.Json;

namespace Compent.Uintra.Core.Sync.Models
{
    public class GoogleAuthResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}