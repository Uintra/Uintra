using System;
using System.Collections.Generic;
using Uintra.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using System.Linq;
using Uintra.Core.Constants;
using Uintra.Tagging.UserTags.Models;

namespace Uintra.Tagging.UserTags
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
            var umbracoNode = _umbracoHelper.TypedContent(tagId);
            return Map(umbracoNode);
        }

        public virtual IEnumerable<UserTag> Get(IEnumerable<Guid> tagIds)
        {
            return GetAll().Join(tagIds, t => t.Id, id => id, (t, _) => t);
        }

        public virtual IEnumerable<UserTag> GetAll()
        {
            return _umbracoHelper
                .TypedContentAtXPath(_xPathProvider.UserTagFolderXPath)
                .Select(Map);
        }

        protected virtual UserTag Map(IPublishedContent userTag)
        {
            var id = userTag.GetKey();
            var text = userTag.GetPropertyValue<string>(UmbracoAliases.Tags.TagText);
            return new UserTag(id, text);
        }
    }
}