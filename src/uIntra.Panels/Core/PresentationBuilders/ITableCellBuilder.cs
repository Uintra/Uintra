using System.Collections.Generic;
using Uintra.Panels.Core.Models.Table;

namespace Uintra.Panels.Core.PresentationBuilders
{
    public interface ITableCellBuilder
    {
        List<List<CellViewModel>> Map(IEnumerable<IEnumerable<CellModel>> rows, bool makeFirstColumnBold);
    }
}