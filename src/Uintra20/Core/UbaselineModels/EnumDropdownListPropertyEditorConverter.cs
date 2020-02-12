using UBaseline.Core.PropertyEditor;
using UBaseline.Shared.Node;
using UBaseline.Shared.Property;

namespace Uintra20.Core.UbaselineModels
{
    public class EnumDropdownListPropertyEditorConverter : PropertyEditorConverter<string, string>
    {
        public override string[] PropertyEditorAliases => new[] { "EnumDropdownList" };

        protected override string GetViewModelValue(INodeModel content, PropertyModel<string> property)
        {
            return property.Value;
        }
    }
}