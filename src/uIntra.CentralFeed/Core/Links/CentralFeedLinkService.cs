using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core;
using uIntra.Core.Extentions;
using uIntra.Core.Links;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uIntra.CentralFeed
{
    public class CentralFeedLinkService : ICentralFeedLinkService
    {
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly IIntranetUserContentHelper _intranetUserContentHelper;
        private readonly UmbracoHelper _umbracoHelper;

        public CentralFeedLinkService(IDocumentTypeAliasProvider documentTypeAliasProvider, UmbracoHelper umbracoHelper, IIntranetUserContentHelper intranetUserContentHelper)
        {
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _umbracoHelper = umbracoHelper;
            _intranetUserContentHelper = intranetUserContentHelper;
        }

        private string GetPageUrl(string xPath) =>
            _umbracoHelper.TypedContentSingleAtXPath(xPath).Url;

        public ActivityLinks GetLinks(IFeedItem item)
        {
            return new ActivityLinks(
                overview: GetOverviewPage(item),
                create: GetCreatePage(item.Type),
                details: GetDetailsPage(item),
                edit: GetEditPage(item),
                creator: GetCreatorPage(item.CreatorId),
                detailsNoId: GetDetailsNoId(item.Type)
                );
        }

        private string GetEditPage(IFeedItem item)
        {
            var xPath = new[]
            {
                _documentTypeAliasProvider.GetHomePage(),
                _documentTypeAliasProvider.GetOverviewPage(item.Type),
                _documentTypeAliasProvider.GetEditPage(item.Type)
            };
            return GetPageUrl(XPathHelper.GetXpath(xPath)).AddIdParameter(item.Id);
        }

        private string GetDetailsPage(IFeedItem item)
        {
            var xPath = new[]
            {
                _documentTypeAliasProvider.GetHomePage(),
                _documentTypeAliasProvider.GetOverviewPage(item.Type),
                _documentTypeAliasProvider.GetDetailsPage(item.Type)
            };
            return GetPageUrl(XPathHelper.GetXpath(xPath)).AddIdParameter(item.Id);
        }

        private string GetCreatorPage(Guid creatorId)
        {
            var profile = _intranetUserContentHelper.GetProfilePage().Url.AddIdParameter(creatorId);
            return profile;
        }

        private string GetDetailsNoId(IIntranetType type)
        {
            var xPath = new[]
            {
                _documentTypeAliasProvider.GetHomePage(),
                _documentTypeAliasProvider.GetOverviewPage(type),
                _documentTypeAliasProvider.GetDetailsPage(type)
            };
            return GetPageUrl(XPathHelper.GetXpath(xPath));
        }

        private string GetCreatePage(IIntranetType type)
        {
            var xPath = new[]
            {
                _documentTypeAliasProvider.GetHomePage(),
                _documentTypeAliasProvider.GetOverviewPage(type),
                _documentTypeAliasProvider.GetCreatePage(type)
            };
            return GetPageUrl(XPathHelper.GetXpath(xPath));
        }

        private string GetOverviewPage(IFeedItem item)
        {
            var xPath = new[]
            {
                _documentTypeAliasProvider.GetHomePage(),
                _documentTypeAliasProvider.GetOverviewPage(item.Type),
            };
            return GetPageUrl(XPathHelper.GetXpath(xPath));
        }

        public ActivityLinks GetCreateLinks()
        {
            throw new NotImplementedException();
        }
    }



}