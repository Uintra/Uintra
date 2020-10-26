using System.Collections.Generic;
using System.Linq;
using Compent.Extensions;
using UBaseline.Core.Extensions;
using UBaseline.Core.PropertyEditor;
using UBaseline.Shared.Node;
using UBaseline.Shared.Property;
using Uintra20.Features.Tagging.UserTags.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Uintra20.Features.Tagging.UserTags.Converters
{
    public class TagsPropertyEditorConverter:PropertyEditorConverter<IEnumerable<UserTagPanelModel>, IEnumerable<string>>
    {
        public override string[] PropertyEditorAliases =>new[] { "userTags" };
        
        protected override IEnumerable<string> GetViewModelValue(
            INodeModel content, 
            PropertyModel<IEnumerable<UserTagPanelModel>> property)
        {
            return property.Value?.Select(v => v.Name);
        }

        protected override IEnumerable<UserTagPanelModel> GetValue(IPublishedProperty property, string culture = null)
        {
            var value = property.Value<string>(culture);
            if (!value.HasValue())
                return null;

            var model = value.Deserialize<IEnumerable<UserTagPanelModel>>();
            return model;
        }
    }
}