using System.Collections.Generic;

namespace uCommunity.Navigation
{
    public class NavigationConfiguration
    {
        // Document Type Aliases
        public string HomePageAlias { get; set; }
        public List<string> Exclude { get; set; }

        // Document Type Property Aliases and Default Values
        public string IsHideFromNavigationAlias { get; set; }
        public bool IsHideFromNavigationDefaultValue { get; set; }

        public string IsShowInLeftNavigationAlias { get; set; }
        public bool IsShowInLeftNavigationDefaultValue { get; set; }

        public string IsShowInHomeNavigationAlias { get; set; }
        public bool IsShowInHomeNavigationDefaultValue { get; set; }

        public string IsShowInSubNavigationAlias { get; set; }
        public bool IsShowInSubNavigationDefaultValue { get; set; }

        public string NavigationNameAlias { get; set; }

        public NavigationConfiguration()
        {
            Exclude = new List<string>();
        }
    }
}
