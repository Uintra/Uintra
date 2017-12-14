using System;
using System.Collections.Generic;
using uIntra.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using System.Linq;
using uIntra.Tagging.UserTags.Models;

namespace uIntra.Tagging.UserTags
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

        public UserTag Get(Guid tagId)
        {
            var umbracoNode = _umbracoHelper.TypedContent(tagId);
            return Map(umbracoNode);
        }

        public IEnumerable<UserTag> Get(IEnumerable<Guid> tagIds)
        {
            return GetAll().Join(tagIds, t => t.Id, id => id, (t, _) => t);
        }

        public IEnumerable<UserTag> GetAll()
        {
            return _umbracoHelper
                .TypedContentAtXPath(_xPathProvider.UserTagFolderXPath)
                .Select(Map);
        }

        protected virtual UserTag Map(IPublishedContent userTag)
        {
            var id = userTag.GetKey();
            var text = userTag.GetPropertyValue<string>("text"); // todo: move to constants
            return new UserTag(id, text);
        }
    }
}