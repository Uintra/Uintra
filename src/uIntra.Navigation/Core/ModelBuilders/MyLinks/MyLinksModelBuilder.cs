using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Activity;
using uIntra.Core.Configuration;
using uIntra.Core.Extentions;
using uIntra.Core.User;
using uIntra.Navigation.Configuration;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uIntra.Navigation.MyLinks
{
    public class MyLinksModelBuilder : NavigationModelBuilderBase<IEnumerable<MyLinkItemModel>>, IMyLinksModelBuilder
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IMyLinksService _myLinksService;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly INavigationApplicationSettings _navigationApplicationSettings;

        public MyLinksModelBuilder(
            UmbracoHelper umbracoHelper,
            IConfigurationProvider<NavigationConfiguration> navigationConfigurationProvider,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IMyLinksService myLinksService,
            IActivitiesServiceFactory activitiesServiceFactory, 
            INavigationApplicationSettings navigationApplicationSettings)
            : base(umbracoHelper, navigationConfigurationProvider)
        {
            _umbracoHelper = umbracoHelper;
            _intranetUserService = intranetUserService;
            _myLinksService = myLinksService;
            _activitiesServiceFactory = activitiesServiceFactory;
            _navigationApplicationSettings = navigationApplicationSettings;
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
                    Name = link.ActivityId.HasValue ? GetActivityLinkName(link.ActivityId.Value) : GetNavigationName(content),
                    Url = GetUrl(link, content)
                });

            return models.Distinct();
        }

        private string GetActivityLinkName(Guid activityId)
        {
            var service = _activitiesServiceFactory.GetService<IIntranetActivityService<IIntranetActivity>>(activityId);
            var activity = service.Get(activityId);

            if (activity.Type.Id == IntranetActivityTypeEnum.Bulletins.ToInt())
            {
                var lengthForPreview = _navigationApplicationSettings.MyLinksBulletinsTitleLength;
                var description = activity.Description.RemoveHtmlTags();
                return description.Length > lengthForPreview ? description.Substring(0, lengthForPreview) + "..." : description;
            }

            return activity.Title;
        }

        private static string GetUrl(MyLink link, IPublishedContent content)
        {
            if (link.QueryString.IsNullOrEmpty())
            {
                return content.Url;
            }

            return $"{content.Url}?{link.QueryString}";
        }

        protected override bool IsHideFromNavigation(IPublishedContent publishedContent)
        {
            throw new System.NotImplementedException();
        }
    }
}
