using Compent.LinkPreview.HttpClient.Extensions;
using UBaseline.Core.PropertyEditor;
using UBaseline.Shared.Node;
using UBaseline.Shared.Property;
using Uintra20.Features.Navigation.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Uintra20.Core.UbaselineModels
{
    public class LinksPickerPropertyEditorConverter : PropertyEditorConverter<LinksPicker[], LinksPickerViewModel[]>
    {
        public LinksPickerPropertyEditorConverter() : base()
        {
        }

        public override string[] PropertyEditorAliases => new[] {"custom.LinksPicker"};

        protected override LinksPickerViewModel[] GetViewModelValue(INodeModel content, PropertyModel<LinksPicker[]> property)
        {
            return null;
        }

        protected override LinksPicker[] GetValue(IPublishedProperty property, string culture = null)
        {
            var test = property.Value<string>(culture);

            return test.Deserialize<LinksPicker[]>();
        }
    }
}