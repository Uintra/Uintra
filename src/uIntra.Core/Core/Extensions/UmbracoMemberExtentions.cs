using System;
using LanguageExt;
using Umbraco.Core;
using Umbraco.Core.Models;
using  static LanguageExt.Prelude;

namespace Uintra.Core.Extensions
{
    public static class UmbracoMemberExtensions
    {
        public static TValue GetValueOrDefault<TValue>(this IMember member, string alias)
        {
            return member.HasProperty(alias) ? member.GetValue<TValue>(alias) : default;
        }

        public static Option<TValue> GetValueOrNone<TValue>(this IMember member, string alias) => 
            member.HasProperty(alias) && member.GetValue<TValue>(alias) is TValue value ? Some(value) : None;

        public static Option<int> GetMemberImageId(this IMember member, string alias)
        {
            if (member.HasProperty(alias))
            {
                var imageString = (string) member.GetValue(alias);
                if (GuidUdi.TryParse(imageString, out var imageGuidUdi))
                {
                    var imageNodeId = ApplicationContext.Current.Services.EntityService.GetIdForKey(
                        imageGuidUdi.Guid,
                        (UmbracoObjectTypes) Enum.Parse(typeof(UmbracoObjectTypes),
                            imageGuidUdi.EntityType,
                            ignoreCase: true));
                    return imageNodeId.Result;
                }
            }

            return None;
        }
    }
}