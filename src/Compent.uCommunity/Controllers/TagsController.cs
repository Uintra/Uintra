using uCommunity.Tagging;
using uCommunity.Tagging.Web;

namespace Compent.uCommunity.Controllers
{
    public class TagsController : TagsControllerBase
    {
        public TagsController(ITagsService tagsService)
            : base(tagsService)
        {
        }
    }
}