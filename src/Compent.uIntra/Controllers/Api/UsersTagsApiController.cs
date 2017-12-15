using uIntra.Tagging.UserTags;
using uIntra.Tagging.Web;
using Umbraco.Web;

namespace Compent.uIntra.Controllers.Api
{
    public class UsersTagsApiController : UserTagsApiControllerBase
    {
        public UsersTagsApiController(
            UserTagService userTagService,
            UmbracoHelper umbracoHelper,
            IUserTagProvider userTagProvider) : base(userTagService, umbracoHelper, userTagProvider)
        {
        }
    }
}