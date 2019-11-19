using System;
using System.Collections.Generic;
using System.Linq;
using Uintra20.Features.Tagging.UserTags.Models;
using Uintra20.Infrastructure.Constants;
using Uintra20.Infrastructure.Providers;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Uintra20.Features.Tagging.UserTags.Services
{
    public class UserTagProvider : IUserTagProvider
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IXPathProvider _xPathProvider;

        public UserTagProvider(UmbracoHelper umbracoHelper, IXPathProvider xPathProvider)
        {
            _umbracoHelper = umbracoHelper;
            _xPathProvider = xPathProvider;
        }

        public virtual UserTag Get(Guid tagId)
        {
            var umbracoNode = _umbracoHelper.Content(tagId);
            return Map(umbracoNode);
        }

        public virtual IEnumerable<UserTag> Get(IEnumerable<Guid> tagIds)
        {
            return GetAll().Join(tagIds, t => t.Id, id => id, (t, _) => t);
        }

        public virtual IEnumerable<UserTag> GetAll()
        {
            return _umbracoHelper
                .ContentAtXPath(_xPathProvider.UserTagFolderXPath)
                .Select(Map);
        }

        protected virtual UserTag Map(IPublishedContent userTag)
        {
            var id = userTag.Key;
            var text = userTag.Value<string>(UmbracoAliases.Tags.TagText);
            return new UserTag(id, text);
        }
    }
}