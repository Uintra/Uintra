using System.Collections.Generic;

namespace Uintra.Navigation.Configuration
{
    public class NavigationConfiguration
    {
        // Document Type Aliases
        public string HomePageAlias { get; set; }
        public List<string> Exclude { get; set; }

        // Document Type Property Aliases and Default Values
        public NavigationPropertySettings<bool> IsHideFromLeftNavigation { get; set; }
        public NavigationPropertySettings<bool> IsShowInHomeNavigation { get; set; }
        public NavigationPropertySettings<bool> IsHideFromSubNavigation { get; set; }

        public NavigationItemTypeSettings NavigationName { get; set; }
        public NavigationItemTypeSettings NavigationComposition { get; set; }
        public NavigationItemTypeSettings HomeNavigationComposition { get; set; }

        public string NavigationTab { get; set; }

        public NavigationConfiguration()
        {
            Exclude = new List<string>();
        }
    }
}
