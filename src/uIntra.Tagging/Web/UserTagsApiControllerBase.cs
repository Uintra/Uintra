using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using uIntra.Tagging.UserTags;
using uIntra.Tagging.UserTags.Models;
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace uIntra.Tagging.Web
{
    public abstract class UserTagsApiControllerBase : UmbracoAuthorizedApiController
    {
        private readonly UserTagService _userTagService;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IUserTagProvider _userTagProvider;

        protected UserTagsApiControllerBase(UserTagService userTagService, UmbracoHelper umbracoHelper, IUserTagProvider userTagProvider)
        {
            _userTagService = userTagService;
            _umbracoHelper = umbracoHelper;
            _userTagProvider = userTagProvider;
        }

        [HttpGet]
        public IEnumerable<UserTagBackofficeViewModel> GetAll(int pageId)
        {
            var content = _umbracoHelper.TypedContent(pageId);

            var allTags = _userTagProvider.GetAll();

            var selectedTagsDictionary = _userTagService
                .Get(content.GetKey())
                .ToDictionary(tag => tag.Id);

            var result = allTags.Select(tag => new UserTagBackofficeViewModel
            {
                Id = tag.Id,
                Text = tag.Text,
                Selected = selectedTagsDictionary.ContainsKey(tag.Id)
            });

            return result;
        }
    }
}
