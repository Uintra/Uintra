
using Compent.Uintra.Core.Verification;

namespace Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants
{
    public class MailInstallationConstants
    {
        [UmbracoDocumentTypeVerification]
        public class DocumentTypeAliases
        {
            public const string MailTemplatesFolder = "mailTemplatesFolder";
        }

        public class ContentDefaultName
        {
            public const string MailTemplatesFolder = "Mail Templates Folder";
        }
    }
}