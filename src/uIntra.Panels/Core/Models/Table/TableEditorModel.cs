using System.Collections.Generic;
using System.Linq;

namespace Uintra.Panels.Core.Models.Table
{
    public class TableEditorModel
    {
        public string Title { get; set; }

        public bool UseFirstRowAsHeader { get; set; }        

        public bool MakeFirstColumnBold { get; set; }

        public IEnumerable<IEnumerable<dynamic>> Cells { get; set; } = Enumerable.Empty<IEnumerable<IEnumerable<dynamic>>>();
    }
}