using uIntra.Core.Extensions;
using uIntra.Panels.Core.TableEditor.PresentationBuilders;
using uIntra.Panels.Core.TablePanel.ModelBuilders;
using uIntra.Panels.Core.TablePanel.Models;
using Umbraco.Core.Models;

namespace uIntra.Panels.Core.TablePanel.PresentationBuilders
{
    public class TablePanelPresentationBuilder : ITablePanelPresentationBuilder
    {
        private readonly ITablePanelModelBuilder _tablePanelModelBuilder;
        private readonly ITableEditorPresentationBuilder _tableEditorPresentationBuilder;

        public TablePanelPresentationBuilder(
            ITablePanelModelBuilder tablePanelModelBuilder,
            ITableEditorPresentationBuilder tableEditorPresentationBuilder)
        {
            this._tablePanelModelBuilder = tablePanelModelBuilder;
            this._tableEditorPresentationBuilder = tableEditorPresentationBuilder;
        }

        public virtual string ContentTypeAlias => _tablePanelModelBuilder.ContentTypeAlias;

        public virtual TablePanelViewModel Get(IPublishedContent publishedContent)
        {
            var model = _tablePanelModelBuilder.Get(publishedContent);

            var result = Map(model);

            return result;
        }

        protected virtual TablePanelViewModel Map(TablePanelModel model)
        {
            var result = new TablePanelViewModel
            {
                Title = model.Title,
                Table = _tableEditorPresentationBuilder.Map(model.Table),
                PanelBehavior = model.BackgroundBehavior.Map<TableBehaviorViewModel>(),
                PanelColors = model.PanelColors.Map<TablePanelColorsViewModel>(),
            };

            return result;
        }
    }
}