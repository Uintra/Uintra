using Compent.Uintra.Core.Verification;

namespace Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants
{
    public class BulletinsInstallationConstants
    {
        public class DocumentTypeNames
        {
            public const string BulletinsDetailsPage = "Bulletins Details Page";
            public const string BulletinsEditPage = "Bulletins Edit Page";
            public const string BulletinsOverviewPage = "Bulletins Overview Page";
        }

        [UmbracoDocumentTypeVerification]
        public class DocumentTypeAliases
        {
            [UmbracoDocumentTypeVerification]
            public const string BulletinsDetailsPage = "bulletinsDetailsPage";
            public const string BulletinsEditPage = "bulletinsEditPage";
            public const string BulletinsOverviewPage = "bulletinsOverviewPage";
        }

        public class DocumentTypeIcons
        {
            public const string BulletinsDetailsPage = "icon-eye";
            public const string BulletinsEditPage = "icon-edit";
            public const string BulletinsOverviewPage = "icon-notepad";
        }
    }
}
