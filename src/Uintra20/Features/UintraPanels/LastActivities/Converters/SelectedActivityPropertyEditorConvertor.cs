using Compent.Shared.Extensions;
using Compent.Shared.Extensions.Bcl;
using UBaseline.Core.PropertyEditor;
using UBaseline.Shared.Node;
using UBaseline.Shared.Property;
using Uintra20.Features.UintraPanels.LastActivities.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Uintra20.Features.UintraPanels.LastActivities.Converters
{
    public class SelectedActivityPropertyEditorConvertor : PropertyEditorConverter<SelectedActivityModel, SelectedActivityViewModel>
    {
        public override string[] PropertyEditorAliases => new[] { "availableActivitySelector" };

        protected override SelectedActivityModel GetValue(IPublishedProperty property, string culture = null)
        {
            var value = property.Value<string>(culture);
            if (!value.HasValue())
                return null;

            var model = value.Deserialize<SelectedActivityModel>();
            return model;
        }
        protected override SelectedActivityViewModel GetViewModelValue(INodeModel content, PropertyModel<SelectedActivityModel> property)
        {
            return new SelectedActivityViewModel
            {
                ActivityId = property.Value.Id,
                ActivityName = property.Value.Name
            };
        }

    }
}