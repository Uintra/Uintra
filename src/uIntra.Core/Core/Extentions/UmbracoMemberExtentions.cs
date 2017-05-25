using Umbraco.Core.Models;

namespace uIntra.Core.Extentions
{
    public static class UmbracoMemberExtentions
    {
        public static TValue GetValueOrDefault<TValue>(this IMember member, string alias)
        {
            if (member.HasProperty(alias))
            {
                return member.GetValue<TValue>(alias);
            }

            return default(TValue);
        }
    }
}