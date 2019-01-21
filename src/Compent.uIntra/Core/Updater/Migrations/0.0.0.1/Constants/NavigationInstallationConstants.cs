using Compent.Uintra.Core.Verification;

namespace Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants
{
    public class NavigationInstallationConstants
    {
        public class DocumentTypeNames
        {
            public const string SystemLink = "Shared Link";
            public const string SystemLinkFolder = "Shared Link Folder";
            public const string HomeNavigationComposition = "Home Navigation Composition";
            public const string NavigationComposition = "Navigation Composition";
        }
        [UmbracoDocumentTypeVerification]
        public class DocumentTypeAliases
        {
            public const string SystemLink = "systemLink";
            public const string SystemLinkFolder = "systemLinkFolder";
            public const string HomeNavigationComposition = "homeNavigationComposition";
            public const string NavigationComposition = "navigationComposition";
        }
        public class DocumentTypePropertyNames
        {
            public const string LinksGroupTitle = "Links Group Title";
            public const string Sort = "Sort";
            public const string Links = "Links";
            public const string NavigationName = "Navigation Name";
            public const string HideFromLeftNavigation = "Hide from Left Navigation";
            public const string HideFromSubNavigation = "Hide from Sub Navigation";
            public const string IsShowInHomeNavigation = "Is show in Home Navigation";
        }
        public class DocumentTypePropertyAliases
        {
            public const string LinksGroupTitle = "linksGroupTitle";
            public const string Sort = "sort";
            public const string Links = "links";
            public const string NavigationName = "navigationName";
            public const string IsHideFromLeftNavigation = "isHideFromLeftNavigation";
            public const string IsHideFromSubNavigation = "isHideFromSubNavigation";
            public const string IsShowInHomeNavigation = "isShowInHomeNavigation";
        }

        public class DocumentTypeIcons
        {
            public const string NavigationComposition = "icon-folder";
            public const string HomePage = "icon-home";
        }

        public class DocumentTypeTabNames
        {
            public const string Navigation = "Navigation";
            public const string Links = "Navigation";
        }

        public class DataTypeNames
        {
            public const string LinksPicker = "Links Picker";
            public const string IsShowInHomeNavigationTrueFalse = "Is show in Home Navigation - TrueFalse";
            public const string IsHideFromLeftNavigation = "Is hide from Left Navigation - TrueFalse";
            public const string IsHideFromSubNavigation = "Is hide from Sub Navigation - TrueFalse";
        }

        public class DataTypePreValueAliases
        {
            public const string PanelPickerConfig = "config";
        }

        public class DataTypePropertyEditors
        {
            public const string LinksPicker = "custom.LinksPicker";
        }
        public class DataTypePropertyAliases
        {
            public const string Assembly = "assembly";
            public const string Enum = "enum";
        }

        public class ContentDefaultName
        {
            public const string SystemLinkFolder = "System Link Folder";
        }
    }
}
