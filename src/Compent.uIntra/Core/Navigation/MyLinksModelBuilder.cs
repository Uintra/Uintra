using System;
using System.Collections.Generic;
using System.Linq;
using Compent.Extensions;
using Uintra.Core.Activity;
using Uintra.Core.Configuration;
using Uintra.Core.Extensions;
using Uintra.Core.User;
using Uintra.Groups;
using Uintra.Navigation;
using Uintra.Navigation.Configuration;
using Uintra.Navigation.MyLinks;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Compent.Uintra.Core.Navigation
{
    public class MyLinksModelBuilder : NavigationModelBuilderBase<IEnumerable<MyLinkItemModel>>, IMyLinksModelBuilder
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly IMyLinksService _myLinksService;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly INavigationApplicationSettings _navigationApplicationSettings;
        private readonly IGroupService _groupService;
        private readonly IEqualityComparer<MyLinkItemModel> _myLinkItemModelComparer;

        public MyLinksModelBuilder(
            UmbracoHelper umbracoHelper,
            IConfigurationProvider<NavigationConfiguration> navigationConfigurationProvider,
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            IMyLinksService myLinksService,
            IActivitiesServiceFactory activitiesServiceFactory,
            INavigationApplicationSettings navigationApplicationSettings,
            IGroupService groupService,
            IEqualityComparer<MyLinkItemModel> myLinkItemModelComparer)
            : base(umbracoHelper, navigationConfigurationProvider)
        {
            _umbracoHelper = umbracoHelper;
            _intranetMemberService = intranetMemberService;
            _myLinksService = myLinksService;
            _activitiesServiceFactory = activitiesServiceFactory;
            _navigationApplicationSettings = navigationApplicationSettings;
            _groupService = groupService;
            _myLinkItemModelComparer = myLinkItemModelComparer;
        }

        public override IEnumerable<MyLinkItemModel> GetMenu()
        {
            var links = _myLinksService
                .GetMany(_intranetMemberService.GetCurrentMember().Id)
                .OrderByDescending(link => link.CreatedDate)
                .ToList();

            var contents = _umbracoHelper.TypedContent(links.Select(el => el.ContentId));

            var models = links.Join(contents,
                link => link.ContentId,
                content => content.Id,
                (link, content) => new MyLinkItemModel
                {
                    Id = link.Id,
                    Name = link.ActivityId.HasValue ? GetLinkName(link.ActivityId.Value) : GetNavigationName(content),
                    Url = GetUrl(link, content)
                });

            return models.Distinct(_myLinkItemModelComparer);
        }

        protected string GetLinkName(Guid entityId)
        {
            var linkName = GetGroupLink(entityId);
            if (linkName.IsNullOrEmpty())
            {
                return GetActivityLink(entityId);
            }

            return linkName;
        }

        private string GetActivityLink(Guid entityId)
        {
            var service = _activitiesServiceFactory.GetService<IIntranetActivityService<IIntranetActivity>>(entityId);

            var activity = service.Get(entityId);

            if (activity.Type is IntranetActivityTypeEnum.Bulletins)
            {
                var lengthForPreview = _navigationApplicationSettings.MyLinksBulletinsTitleLength;
                var description = activity.Description.StripHtml();
                return description.Length > lengthForPreview ? description.Substring(0, lengthForPreview) + "..." : description;
            }

            return activity.Title;
        }

        private string GetGroupLink(Guid entityId)
        {
            var groupModel = _groupService.Get(entityId);
            return groupModel?.Title;
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
