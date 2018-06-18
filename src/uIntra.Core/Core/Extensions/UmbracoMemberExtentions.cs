using System;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace uIntra.Core.Extensions
{
    public static class UmbracoMemberExtensions
    {
        public static TValue GetValueOrDefault<TValue>(this IMember member, string alias)
        {
            return member.HasProperty(alias) ? member.GetValue<TValue>(alias) : default(TValue);
        }

        public static int? GetMemberImageId(this IMember member, string alias)
        {
            if (member.HasProperty(alias))
            {
                var imageString = (string) member.GetValue(alias);
                GuidUdi imageGuidUdi;
                if (GuidUdi.TryParse(imageString, out imageGuidUdi))
                {
                    var imageNodeId = ApplicationContext.Current.Services.EntityService.GetIdForKey(
                        imageGuidUdi.Guid,
                        (UmbracoObjectTypes) Enum.Parse(typeof(UmbracoObjectTypes),
                            imageGuidUdi.EntityType,
                            ignoreCase: true));
                    return imageNodeId.Result;
                }
            }

            return null;
        }
    }
}