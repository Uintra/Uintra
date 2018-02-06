using System.Web.Mvc;
using uIntra.Panels.Core.TablePanel.PresentationBuilders;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace uIntra.Panels.Core.TablePanel.Controllers
{
    public class TablePanelController: SurfaceController
    {
        protected virtual string ViewPath => @"\uIntra.Panels\Core\TablePanel\Views\TablePanel.cshtml";
        private readonly ITablePanelPresentationBuilder _tablePanelPresentationBuilder;
        private readonly UmbracoHelper _umbracoHelper;

        public TablePanelController(
            ITablePanelPresentationBuilder tablePanelPresentationBuilder,
            UmbracoHelper umbracoHelper
            )
        {
            _tablePanelPresentationBuilder = tablePanelPresentationBuilder;
            _umbracoHelper = umbracoHelper;
        }

        public virtual PartialViewResult RenderInline(IPublishedContent publishedContent)
        {
            var result = _tablePanelPresentationBuilder.Get(publishedContent);
            return PartialView(ViewPath, result);
        }

        public virtual ActionResult Render(int id)
        {
            var publishedContent = _umbracoHelper.TypedContent(id);
            if (publishedContent == null)
            {
                return new EmptyResult();
            }

            var result = _tablePanelPresentationBuilder.Get(publishedContent);
            return PartialView(ViewPath, result);
        }
    }
}