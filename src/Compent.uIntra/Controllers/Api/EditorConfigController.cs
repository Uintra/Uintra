using Uintra.Core.Controls;
using Uintra.Core.Web;

namespace Compent.Uintra.Controllers.Api
{
    public class EditorConfigController : EditorConfigControllerBase
    {
        public EditorConfigController(IEditorConfigProvider configProvider) : base(configProvider)
        {
        }
    }
}