using System;
using Umbraco.Core.Dashboards;

namespace Uintra20.App_Plugins.Utils.Search
{
    public class SearchDashboard : IDashboard
    {
        public string Alias { get; } = "Search";
        public string View { get; } = "/App_Plugins/Utils/Search/search.html";
        public string[] Sections { get; } = { "settings" };
        public IAccessRule[] AccessRules { get; } = Array.Empty<IAccessRule>();
    }
}