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
            table.Cells = _tableCellBuilder.Map(rows, model.MakeFirstColumnBold);

            var result = new TablePanelViewModel
            {
                Table = table
            };

            return result;
        }

        private List<List<CellModel>> CollectRows(IEnumerable<IEnumerable<dynamic>> cells)
        {
            var rows = new List<List<CellModel>>();
            foreach (var cellItems in cells)
            {
                var row = cellItems.Select(item => new CellModel { Value = item.value }).ToList();
                rows.Add(row);
            }

            return rows;
        }
    }
}