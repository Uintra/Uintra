using System.Web;
using uIntra.Core;
using uIntra.Core.Extentions;

namespace Compent.uIntra.Core.Notification
{
    public class MailConfiguration
    {

        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        static MailConfiguration()
        {
            var docTypeAliasProvider = HttpContext.Current.GetService<IDocumentTypeAliasProvider>();

            MailTemplateXpath = XPathHelper.GetXpath(
            docTypeAliasProvider.GetDataFolder(),
            docTypeAliasProvider.GetMailTemplateFolder(),
            docTypeAliasProvider.GetMailTemplate());
        }
        public static string MailTemplateXpath { get; set; }
    }
    
}