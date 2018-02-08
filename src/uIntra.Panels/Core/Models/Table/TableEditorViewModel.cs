using System.Collections.Generic;

namespace uIntra.Panels.Core.Models.Table
{
    public class TableEditorViewModel
    {
        public bool UseFirstRowAsHeader { get; set; }

        //public bool UseLastRowAsFooter { get; set; }

        public string TableStyle { get; set; }

        public List<string> ColumnStyles { get; set; }

        public List<string> RowStyles { get; set; }

        public List<List<CellViewModel>> Cells { get; set; }
    }
}