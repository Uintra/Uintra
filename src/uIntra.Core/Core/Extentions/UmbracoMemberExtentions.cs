using System;
using Umbraco.Core;
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

        public static int? GetMemberImageId(this IMember member, string alias)
        {
            if (member.HasProperty(alias))
            {
                var imageString = (string)member.GetValue(alias);
                GuidUdi imageGuidUdi;
                if (GuidUdi.TryParse(imageString, out imageGuidUdi))
                {
                    var imageNodeId = ApplicationContext.Current.Services.EntityService.GetIdForKey(imageGuidUdi.Guid, (UmbracoObjectTypes)Enum.Parse(typeof(UmbracoObjectTypes), imageGuidUdi.EntityType, true));
                    return imageNodeId.Result;
                }
            }

            return null;
        }

    }
}