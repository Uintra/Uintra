using Uintra.Tagging.UserTags;
using Uintra.Tagging.Web;
using Umbraco.Web;

namespace Compent.Uintra.Controllers.Api
{
    public class UserTagsApiController : UserTagsApiControllerBase
    {
        public UserTagsApiController(
            UserTagService userTagService,
            UmbracoHelper umbracoHelper,
            IUserTagProvider userTagProvider) : base(userTagService, umbracoHelper, userTagProvider)
        {
        }
    }
}