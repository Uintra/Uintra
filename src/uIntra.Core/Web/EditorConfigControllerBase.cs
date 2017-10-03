using System.Web.Http;
using uIntra.Core.Controls;
using Umbraco.Web.WebApi;

namespace uIntra.Core.Web
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
