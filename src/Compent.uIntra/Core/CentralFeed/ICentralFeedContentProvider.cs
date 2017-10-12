using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;

namespace Compent.uIntra.Core.CentralFeed
{
    public interface ICentralFeedContentProvider
    {
        IPublishedContent GetOverviewPage();
    }

    public class CentralFeedContentProvider : ICentralFeedContentProvider
    {
        public IPublishedContent GetOverviewPage()
        {
            return _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(_documentTypeAliasProvider.GetHomePage()));
        }
    }
}