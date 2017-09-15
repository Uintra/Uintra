using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Activity;
using uIntra.Core.TypeProviders;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uIntra.Core.Core.TypeProviders
{
    public class CentralFeedLinksProvider<TActivityType> : IActivityPageProvider<TActivityType> where TActivityType : IIntranetType
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;

        protected List<string> GetOverviewXPath(IIntranetType activityType) 
            => new List<string> { _documentTypeAliasProvider.GetHomePage(), _documentTypeAliasProvider.GetOverviewPage(activityType) };

        public CentralFeedLinksProvider(UmbracoHelper umbracoHelper,
            IDocumentTypeAliasProvider documentTypeAliasProvider)
        {
            _umbracoHelper = umbracoHelper;
            _documentTypeAliasProvider = documentTypeAliasProvider;
        }

        public IPublishedContent GetOverviewPage(TActivityType type)
        {
            return _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(GetPath(type)));
        }

        public IPublishedContent GetDetailsPage(TActivityType type)
        {
            throw new NotImplementedException();
        }

        public IPublishedContent GetCreatePage(TActivityType type)
        {
            throw new NotImplementedException();
        }

        public IPublishedContent GetEditPage(TActivityType type)
        {
            throw new NotImplementedException();
        }


        private string[] GetPath(IIntranetType type, params string[] aliases)
        {
            var basePath = GetOverviewXPath(type);

            if (aliases.Any())
            {
                basePath.AddRange(aliases.ToList());
            }
            return basePath.ToArray();
        }
    }

    public interface IActivityPageProvider<TActivityType>
        where TActivityType : IIntranetType
    {
        IPublishedContent GetOverviewPage(TActivityType type);
        IPublishedContent GetDetailsPage(TActivityType type);
        IPublishedContent GetCreatePage(TActivityType type);
        IPublishedContent GetEditPage(TActivityType type);
    }

    class LinksProviderFactory
    {
        
    }
}
