using uIntra.Tagging;
using uIntra.Tagging.Web;

namespace Compent.uIntra.Controllers
{
    public class TagsController : TagsControllerBase
    {
        public TagsController(ITagsService tagsService)
            : base(tagsService)
        {
        }
    }
}