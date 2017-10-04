using uIntra.Core.Controls;
using uIntra.Core.Web;

namespace Compent.uIntra.Controllers.Api
{
    public class EditorConfigController : EditorConfigControllerBase
    {
        public EditorConfigController(IEditorConfigProvider configProvider) : base(configProvider)
        {
        }
    }
}