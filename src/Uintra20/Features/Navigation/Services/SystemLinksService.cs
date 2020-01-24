using System.Collections.Generic;
using Uintra20.Infrastructure.Extensions;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Uintra20.Features.Navigation.Services
{
    public class SystemLinksService : ISystemLinksService
    {
        private readonly UmbracoHelper _umbracoHelper;

        public SystemLinksService(UmbracoHelper umbracoHelper)
        {
            _umbracoHelper = umbracoHelper;
        }

        public IEnumerable<IPublishedContent> GetMany(IEnumerable<string> aliasPath)
        {
            return _umbracoHelper.GetByAliasPath(aliasPath);
        }
    }
}