using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Uintra.Core.Extensions
{
    public static class SerializationExtensions
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public static dynamic ToDynamic(this object obj)
        {
            var json = obj.ToJson();
            return json.Deserialize<dynamic>();
        }

        public static string ToJson<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None, Settings);
        }

               public static T Deserialize<T>(this string value)
        {
            return string.IsNullOrEmpty(value)
                ? default
                : (T)JsonConvert.DeserializeObject(value, typeof(T), Settings);
        }
    }
}