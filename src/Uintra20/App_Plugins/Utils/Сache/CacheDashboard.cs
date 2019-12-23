using System;
using Umbraco.Core.Dashboards;

namespace Uintra20.App_Plugins.Utils.Сache
{
	public class CacheDashboard : IDashboard
	{
		public string Alias => "Cache";

		public string[] Sections => new[] { "settings" };

		public string View => "/App_Plugins/Utils/Сache/View/developer-cache.html";

		public IAccessRule[] AccessRules => Array.Empty<IAccessRule>();
	}
}