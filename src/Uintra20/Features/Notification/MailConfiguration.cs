using System.Web;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.Helpers;
using Uintra20.Infrastructure.Providers;

namespace Uintra20.Features.Notification
{
    public static class MailConfiguration
    {
        static MailConfiguration()
        {
            var docTypeAliasProvider = HttpContext.Current.GetService<IDocumentTypeAliasProvider>();

            MailTemplateXpath = string.Empty; //XPathHelper.GetXpath(//TODO:Research when mail service is ready
            //docTypeAliasProvider.GetDataFolder(),
            //docTypeAliasProvider.GetMailTemplateFolder(),
            //docTypeAliasProvider.GetMailTemplate());
        }
        public static string MailTemplateXpath { get; set; }
    }
}