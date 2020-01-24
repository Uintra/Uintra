using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Uintra20.Infrastructure.Extensions
{
    public static class UmbracoHelperExtensions
    {
        public static IEnumerable<IPublishedContent> GetByAliasPath(this UmbracoHelper helper, IEnumerable<string> aliasPath)
        {
            var rootContents = helper.ContentAtRoot();

            IEnumerable<IPublishedContent> ByAliasPath(IEnumerable<IPublishedContent> contents, IEnumerable<string> path)
            {
                if (!path.Any())
                {
                    return contents;
                }

                if (!contents.Any())
                {
                    return Enumerable.Empty<IPublishedContent>();
                }

                var alias = path.First();

                var child = contents.Where(x => x.ContentType.Alias == alias).Select(x => x.Children).SelectMany(x => x);

                return ByAliasPath(child, path.Skip(1));
            }

            return ByAliasPath(rootContents, aliasPath);
        }
    }
}