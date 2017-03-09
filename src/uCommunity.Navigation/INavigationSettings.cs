using System.Collections.Generic;

namespace uCommunity.Navigation
{
    public interface INavigationSettings
    {
        // Document types
        string HomePageAlias { get; }
        List<string> Exclude { get; }

        // Property alias
        string IsHideInNavigationAlias { get; }
        bool IsHideInNavigationDefaultValue { get; }

        string IsShowInHomeNavigationAlias { get; }
        bool IsShowInHomeNavigationDefaultValue { get; }

        string NavigationNameAlias { get; set; }
    }
}
