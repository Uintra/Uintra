using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using umbraco.cms.businesslogic;
using Uintra.Core.Extensions;
using Uintra.Panels.Core.Models.Table;
using Uintra.Panels.Core.PresentationBuilders;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace Uintra.Panels.Web
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

            var grid = publishedContent.GetPropertyValue<dynamic>("grid");
            IEnumerable<TableEditorModel> tables = CollectTables(grid);
            var table = tables.FirstOrDefault(t => t.TableId == tableEditorModel.TableId);
            if (table == null)
            {
                return new EmptyResult();
            }
            tableEditorModel.Cells = table.Cells;
            var result = _tablePanelPresentationBuilder.Get(tableEditorModel);
            return PartialView(ViewPath, result);
        }

        public static bool IsEmptySection(dynamic section)
        {
            return section.rows.Count == 0 || section.rows[0].areas[0].controls.Count == 0;
        }

        private IEnumerable<TableEditorModel> CollectTables(dynamic propertyValue)
        {
            for (var i = 0; i < propertyValue.sections.Count; i++)
            {
                var section = propertyValue.sections[i];

                foreach (var row in section.rows)
                {
                    foreach (var area in row.areas)
                    {
                        foreach (var control in area.controls)
                        {
                            if (control != null && control.editor != null && control?.editor.view != null)
                            {
                                string table = control?.value?.ToString();
                                var model = table.Deserialize<TableEditorModel>();
                                if (model.TableId != Guid.Empty)
                                {
                                    yield return model;
                                }

                            }
                        }
                    }
                }
            }
        }
    }
}