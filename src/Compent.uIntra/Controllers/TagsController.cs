using uIntra.Tagging.UserTags;
using uIntra.Tagging.Web;

namespace Compent.uIntra.Controllers
{
    public class TagsController:UserTagsControllerBase
    {
        public TagsController(IUserTagService tagsService, IUserTagProvider tagProvider) : base(tagsService, tagProvider)
        {
        }
    }
}