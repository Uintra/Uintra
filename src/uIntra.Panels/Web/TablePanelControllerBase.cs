using System.Collections.Generic;
using System.Web.Mvc;
using uIntra.Panels.Core.Models.Table;
using uIntra.Panels.Core.PresentationBuilders;
using Uintra.Core.Extensions;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace uIntra.Panels.Web
{
    public abstract class TablePanelControllerBase : SurfaceController
    {
        protected virtual string ViewPath => @"~/App_Plugins/Panels/TablePanel/TablePanel.cshtml";
        private readonly ITablePanelPresentationBuilder _tablePanelPresentationBuilder;
        private readonly UmbracoHelper _umbracoHelper;

        protected TablePanelControllerBase(
            ITablePanelPresentationBuilder tablePanelPresentationBuilder,
            UmbracoHelper umbracoHelper
            )
        {
            _tablePanelPresentationBuilder = tablePanelPresentationBuilder;
            _umbracoHelper = umbracoHelper;
        }
      
        public virtual ActionResult Render(TableEditorModel tableEditorModel)
        {
            var publishedContent = _umbracoHelper.TypedContent(CurrentPage.Id);
            if (publishedContent == null)
            {
                return new EmptyResult();
            }

            var propertyValue = publishedContent.GetPropertyValue<dynamic>("grid");
            string cells = propertyValue.sections[0].rows[0].areas[0].controls[0].value.cells.ToString();

            tableEditorModel.Cells = cells.Deserialize<IEnumerable<IEnumerable<CellModel>>>();


            var result = _tablePanelPresentationBuilder.Get(tableEditorModel);
            return PartialView(ViewPath, result);
        }
    }
}