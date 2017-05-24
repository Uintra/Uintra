using System.Collections.Generic;
using System.Linq;
using uCommunity.Core.Configuration;
using uCommunity.Core.User;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uCommunity.Navigation.Core
{
    public class MyLinksModelBuilder : NavigationModelBuilderBase<IEnumerable<MyLinkItemModel>>, IMyLinksModelBuilder
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IMyLinksService _myLinksService;

        public MyLinksModelBuilder(
            UmbracoHelper umbracoHelper,
            IConfigurationProvider<NavigationConfiguration> navigationConfigurationProvider,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IMyLinksService myLinksService)
            : base(umbracoHelper, navigationConfigurationProvider)
        {
            _umbracoHelper = umbracoHelper;
            _intranetUserService = intranetUserService;
            _myLinksService = myLinksService;
        }

        public override IEnumerable<MyLinkItemModel> GetMenu()
        {
            var links = _myLinksService.GetMany(_intranetUserService.GetCurrentUser().Id).OrderByDescending(link => link.CreatedDate).ToList();
            var contents = _umbracoHelper.TypedContent(links.Select(el => el.ContentId));

            var models = links.Join(contents,
                link => link.ContentId,
                content => content.Id,
                (link, content) => new MyLinkItemModel
                {
                    Id = link.Id,
                    ContentId = link.ContentId,
                    Name = GetNavigationName(content),
                    Url = content.Url
                });

            return models;
        }

        protected override bool IsHideFromNavigation(IPublishedContent publishedContent)
        {
            throw new System.NotImplementedException();
        }
    }
}
