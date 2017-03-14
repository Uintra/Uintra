using System.Collections.Generic;

namespace uCommunity.Navigation.Core
{
    public class NavigationConfiguration
    {
        // Document Type Aliases
        public string HomePageAlias { get; set; }
        public List<string> Exclude { get; set; }

        public string NavigationCompositionAlias { get; set; }
        public string HomeNavigationCompositionAlias { get; set; }

        // Document Type Property Aliases and Default Values
        public NavigationPropertySettings<bool> IsHideFromNavigation { get; set; }
        public NavigationPropertySettings<bool> IsShowInLeftNavigation { get; set; }
        public NavigationPropertySettings<bool> IsShowInHomeNavigation { get; set; }
        public NavigationPropertySettings<bool> IsShowInSubNavigation { get; set; }

        public NavigationPropertySettings<string> NavigationName { get; set; }

        public string NavigationTab { get; set; }

        public NavigationConfiguration()
        {
            Exclude = new List<string>();
        }
    }

    public class NavigationPropertySettings<T>
    {
        public string Alias { get; set; }
        public string Name { get; set; }
        public T DefaultValue { get; set; }
    }
}
