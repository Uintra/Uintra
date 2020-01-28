using System.Linq;
using Compent.LinkPreview.HttpClient.Extensions;
using UBaseline.Core.PropertyEditor;
using UBaseline.Shared.Node;
using UBaseline.Shared.Property;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Uintra20.Core.UbaselineModels
{
    public class LinksPickerPropertyEditorConverter : PropertyEditorConverter<UintraLinksPickerModel[], UintraLinksPickerViewModel[]>
    {
        public override string[] PropertyEditorAliases => new[] {"custom.LinksPicker"};

        protected override UintraLinksPickerViewModel[] GetViewModelValue(INodeModel content, PropertyModel<UintraLinksPickerModel[]> property)
        {
            return property.Value.Select(sharedLink => sharedLink.ToViewModel()).ToArray();
        }

        protected override UintraLinksPickerModel[] GetValue(IPublishedProperty property, string culture = null)
        {
            var test = property.Value<string>(culture);

            return test.Deserialize<UintraLinksPickerModel[]>();
        }
    }
}