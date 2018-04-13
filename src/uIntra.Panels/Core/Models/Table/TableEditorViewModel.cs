using System.Collections.Generic;

namespace Uintra.Panels.Core.Models.Table
{
    public class TableEditorViewModel
    {
        public string Title { get; set; }

        public bool UseFirstRowAsHeader { get; set; }

        public List<List<CellViewModel>> Cells { get; set; } = new List<List<CellViewModel>>();
    }
}