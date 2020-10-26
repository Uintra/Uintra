using UBaseline.Shared.GlobalScriptsComposition;
using UBaseline.Shared.Node;
using UBaseline.Shared.Property;
using UBaseline.Shared.Title;

namespace Uintra20.Core.Login
{
	public class LoginPageModel: NodeModel,
		ITitleContainer,
		IGlobalScriptsComposition
	{
		public PropertyModel<string> Title { get; set; }
		public GlobalScriptsCompositionModel GlobalScripts { get; set; }
	}
}