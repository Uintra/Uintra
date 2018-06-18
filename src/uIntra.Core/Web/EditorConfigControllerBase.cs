using System.Web.Http;
using Uintra.Core.Controls;
using Umbraco.Web.WebApi;

namespace Uintra.Core.Web
{
    public abstract class EditorConfigControllerBase : UmbracoAuthorizedApiController
    {
        private readonly IEditorConfigProvider _configProvider;

        protected EditorConfigControllerBase(IEditorConfigProvider configProvider)
        {
            _configProvider = configProvider;
        }

        [HttpGet]
        public GridEditorConfig Config(string editorAlias)
        {
            return _configProvider.GetConfig(editorAlias);
        }
    }
}
