using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Extensions;
using Uintra.Panels.Core.Models.Table;

namespace Uintra.Panels.Core.PresentationBuilders
{
    public class TablePanelPresentationBuilder : ITablePanelPresentationBuilder
    {
        private readonly ITableCellBuilder _tableCellBuilder;

        public TablePanelPresentationBuilder(ITableCellBuilder tableCellBuilder)
        {
            _tableCellBuilder = tableCellBuilder;
        }

        public TablePanelViewModel Get(TableEditorModel model)
        {
            var table = model.Map<TableEditorViewModel>();

            var rows = CollectRows(model.Cells);
            table.Cells = _tableCellBuilder.Map(rows);

            var result = new TablePanelViewModel
            {
                Table = table
            };

            return result;
        }

        private static List<List<CellModel>> CollectRows(IEnumerable<IEnumerable<dynamic>> cells) => 
            cells.Select(cellItems => cellItems.Select(item => new CellModel {Value = item.value}).ToList()).ToList();
    }
}