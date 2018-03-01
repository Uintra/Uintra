using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using uIntra.Panels.Core.Models;
using uIntra.Panels.Core.Models.Table;

namespace UIntra.Panels.Core.PresentationBuilders
{
    public class TableCellBuilder : ITableCellBuilder
    {
        private const string _boldTagOpen = "<strong>";
        protected virtual string BoldTagOpen => _boldTagOpen;

        private const string _boldTagClose = "</strong>";
        protected virtual string BoldTagClose => _boldTagClose;

        private const string _boldSymbol = "*";
        protected virtual string BoldSymbol => _boldSymbol;

        private const string _alignCenter = "cell-center";
        protected virtual string AlignCenter => _alignCenter;

        private const string _alignLeft = "cell-left";
        protected virtual string AlignLeft => _alignLeft;

        private const string _alignLeftSymbol = "<<";
        protected virtual string AlignLeftSymbol => _alignLeftSymbol;

        private const string _alignRight = "cell-right";
        protected virtual string AlignRight => _alignRight;

        private const string _alignRightSymbol = ">>";
        protected virtual string AlignRightSymbol => _alignRightSymbol;



        public virtual List<List<CellViewModel>> Map(IEnumerable<IEnumerable<CellModel>> rows, bool makeFirstColumnBold)
        {
            var result = rows.Select(Map)
                .ToList();

            if (makeFirstColumnBold)
            {
                foreach (var row in result)
                {
                    var firstColumn = row.First();
                    firstColumn.Value = BoldTagOpen + firstColumn.Value + BoldTagClose;
                }
            }

            return result;
        }

        protected virtual List<CellViewModel> Map(IEnumerable<CellModel> rows)
        {
            var result = rows
                .Select(MapCell)
                .ToList();

            return result;
        }

        protected virtual CellViewModel MapCell(CellModel model)
        {
            var text = Regex.Replace(model.Value, $@"\{BoldSymbol}([^*]+)\{BoldSymbol}",
                match => $"{BoldTagOpen}{match.Groups[1].Value}{BoldTagClose}");

            var result = new CellViewModel
            {
                Value = EjectAlign(text, out var align),
                Align = align
            };

            return result;
        }

        private string EjectAlign(string text, out string align)
        {
            if (TryEjectAlignSymbol(text, AlignRightSymbol, out var result))
            {
                align = AlignRight;
                return result;
            }

            if (TryEjectAlignSymbol(text, AlignLeftSymbol, out result))
            {
                align = AlignLeft;
                return result;
            }

            align = AlignCenter;
            return text;
        }

        private bool TryEjectAlignSymbol(string text, string symbol, out string result)
        {
            var index = text.IndexOf(symbol, StringComparison.InvariantCulture);
            if (index == -1)
            {
                result = string.Empty;
                return false;
            }

            result = text.Remove(index, symbol.Length);
            return true;
        }
    }
}