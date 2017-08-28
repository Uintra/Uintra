using uIntra.Core;
using uIntra.Notification;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Compent.uIntra.Core.Notification
{
    public class NotificationHelper : INotificationHelper
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;

        public NotificationHelper(UmbracoHelper umbracoHelper, IDocumentTypeAliasProvider documentTypeAliasProvider)
        {
            _umbracoHelper = umbracoHelper;
            _documentTypeAliasProvider = documentTypeAliasProvider;
        }

        public IPublishedContent GetNotificationListPage()
        {
            return _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(_documentTypeAliasProvider.GetHomePage(), _documentTypeAliasProvider.GetNotificationPage()));
        }
    }
}