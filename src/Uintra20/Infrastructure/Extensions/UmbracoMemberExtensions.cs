using System;
using System.Web.Mvc;
using LanguageExt;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using static LanguageExt.Prelude;

namespace Uintra20.Infrastructure.Extensions
{
    public static class UmbracoMemberExtensions
    {
        public static TValue GetValueOrDefault<TValue>(this IMember member, string alias)
        {
            return member.HasProperty(alias) ? member.GetValue<TValue>(alias) : default;
        }

        public static Option<int> GetMemberImageId(this IMember member, string alias)
        {
            if (member.HasProperty(alias))
            {
                var imageString = (string)member.GetValue(alias);
                if (GuidUdi.TryParse(imageString, out var imageGuidUdi))
                {
                    var imageNodeId = DependencyResolver.Current.GetService<IEntityService>().GetId(
                        imageGuidUdi.Guid,
                        (UmbracoObjectTypes)Enum.Parse(typeof(UmbracoObjectTypes),
                            imageGuidUdi.EntityType,
                            ignoreCase: true));
                    return imageNodeId.Result;
                }
            }

            return None;
        }
    }
}