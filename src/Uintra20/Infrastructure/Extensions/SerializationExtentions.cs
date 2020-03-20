using UBaseline.Core.Extensions;

namespace Uintra20.Infrastructure.Extensions
{
    public static class SerializationExtensions
    {
        public static dynamic ToDynamic(this object obj)
        {
            var json = obj.ToJson();
            return json.Deserialize<dynamic>();
        }
    }
}