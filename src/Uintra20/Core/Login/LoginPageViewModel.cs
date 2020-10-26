using System;
using UBaseline.Shared.GlobalScriptsComposition;
using UBaseline.Shared.Node;
using UBaseline.Shared.Property;

namespace Uintra20.Core.Login
{
	public class LoginPageViewModel : NodeViewModel
	{
		public PropertyViewModel<string> Title { get; set; }
		public GlobalScriptsCompositionViewModel GlobalScripts { get; set; }
		public Version CurrentIntranetVersion { get; set; }

	}
}