using System.Web.Mvc;
using Uintra.Panels.Core.Models.Table;
using Uintra.Panels.Core.PresentationBuilders;
using Umbraco.Web.Mvc;

namespace Uintra.Panels.Web
{
    public abstract class TablePanelControllerBase : SurfaceController
    {
        protected virtual string ViewPath => @"~/App_Plugins/Panels/TablePanel/TablePanel.cshtml";
        private readonly ITablePanelPresentationBuilder _tablePanelPresentationBuilder;

        protected TablePanelControllerBase(ITablePanelPresentationBuilder tablePanelPresentationBuilder)
        {
            _tablePanelPresentationBuilder = tablePanelPresentationBuilder;
        }

        public virtual ActionResult Render(TableEditorModel tableEditorModel)
        {
            var result = _tablePanelPresentationBuilder.Get(tableEditorModel);
            return PartialView(ViewPath, result);
        }
    }
}