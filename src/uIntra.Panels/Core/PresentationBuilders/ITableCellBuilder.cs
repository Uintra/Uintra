using System.Collections.Generic;
using uIntra.Panels.Core.Models;
using uIntra.Panels.Core.Models.Table;

namespace uIntra.Panels.Core.PresentationBuilders
{
    public interface ITableCellBuilder
    {
        List<List<CellViewModel>> Map(IEnumerable<IEnumerable<CellModel>> rows, bool makeFirstColumnBold);
    }
}