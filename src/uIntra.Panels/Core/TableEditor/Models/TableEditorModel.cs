using System.Collections.Generic;
using System.Linq;

namespace uIntra.Panels.Core.TableEditor.Models
{
    public class TableEditorModel
    {
        public bool UseFirstRowAsHeader { get; set; }

        public bool UseLastRowAsFooter { get; set; }

        public bool MakeFirstColumnBold { get; set; }

        public string TableStyle { get; set; }

        public IEnumerable<string> ColumnStyles { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<string> RowStyles { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<IEnumerable<CellModel>> Cells { get; set; } = Enumerable.Empty<IEnumerable<CellModel>>();
    }
}