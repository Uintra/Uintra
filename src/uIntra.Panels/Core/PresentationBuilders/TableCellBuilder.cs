using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using LanguageExt;
using Uintra.Core.Extensions;
using Uintra.Panels.Core.Models.Table;
using static LanguageExt.Prelude;

namespace Uintra.Panels.Core.PresentationBuilders
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

        private const string _alignCenterSymbol = "<>";
        protected virtual string AlignCenterSymbol => _alignCenterSymbol;

        private const string _alignRight = "cell-right";
        protected virtual string AlignRight => _alignRight;

        private const string _alignRightSymbol = ">>";
        protected virtual string AlignRightSymbol => _alignRightSymbol;

        public virtual List<List<CellViewModel>> Map(IEnumerable<IEnumerable<CellModel>> rows)
        {
            var result = rows.Select(Map)
                .ToList();

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

            var (value, align) = EjectAlign(text);

            var result = new CellViewModel
            {
                Value = value,
                Align = align
            };

            return result;
        }

        private (string result, string align) EjectAlign(string text) =>
            TryEjectAlignSymbol(text, AlignRightSymbol)
                .Choose(() => TryEjectAlignSymbol(text, AlignCenterSymbol))
                .IfNone(() => (text, AlignLeft));

        private static Option<(string text, string align)> TryEjectAlignSymbol(string text, string symbol)
        {
            var index = text.IndexOf(symbol, StringComparison.InvariantCulture);
            return index == -1 ? None : Some((text.Remove(index, symbol.Length), symbol));
        }
    }
}