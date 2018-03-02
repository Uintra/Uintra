using System;
using System.Collections.Generic;
using System.Linq;

namespace Uintra.Panels.Core.Models.Table
{
    public class TableEditorModel
    {
        public Guid TableId { get; set; }

        public string Title { get; set; }

        public bool UseFirstRowAsHeader { get; set; }        

        public bool MakeFirstColumnBold { get; set; }
        
        public IEnumerable<IEnumerable<CellModel>> Cells { get; set; } = Enumerable.Empty<IEnumerable<CellModel>>();
    }
}