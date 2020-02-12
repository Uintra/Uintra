using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Compent.Extensions;
using UBaseline.Core.Node;
using UBaseline.Shared.Node;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Entities;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Navigation.ApplicationSettings;
using Uintra20.Features.Navigation.Models;
using Uintra20.Features.Navigation.Models.MyLinks;
using Uintra20.Features.Navigation.Services;
using Uintra20.Features.Navigation.Sql;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.Providers;
using Uintra20.Infrastructure.TypeProviders;

namespace Uintra20.Features.Navigation
{
    public class MyLinksHelper : IMyLinksHelper
    {
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IMyLinksService _myLinksService;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly INavigationApplicationSettings _navigationApplicationSettings;
        private readonly IGroupService _groupService;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly INodeModelService _nodeModelService;

        public MyLinksHelper(
            IDocumentTypeAliasProvider documentTypeAliasProvider,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IMyLinksService myLinksService,
            IActivitiesServiceFactory activitiesServiceFactory,
            INavigationApplicationSettings navigationApplicationSettings,
            IGroupService groupService,
            INodeModelService nodeModelService,
            IActivityTypeProvider activityTypeProvider)
        {
            _intranetMemberService = intranetMemberService;
            _myLinksService = myLinksService;
            _activitiesServiceFactory = activitiesServiceFactory;
            _navigationApplicationSettings = navigationApplicationSettings;
            _groupService = groupService;
            _nodeModelService = nodeModelService;
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _activityTypeProvider = activityTypeProvider;
        }

        public IEnumerable<MyLinkItemModel> GetMenu()
        {
            var links = _myLinksService
                .GetMany(_intranetMemberService.GetCurrentMember().Id)
                .OrderByDescending(link => link.CreatedDate)
                .ToList();
            
            var contents = _nodeModelService.GetByIds(links.Select(el => el.ContentId));

            var models = links.Join(contents,
                link => link.ContentId,
                content => content.Id,
                (link, content) => new MyLinkItemModel
                {
                    Id = link.Id,
                    ContentId=content.Id,
                    ActivityId=link.ActivityId,
                    Name = link.ActivityId.HasValue ? GetLinkName(link.ActivityId.Value) : GetNavigationName(content),
                    Url = GetUrl(link, content)
                });

            return models;
        }

        public async Task<IEnumerable<MyLinkItemModel>> GetMenuAsync()
        {
            var memberId = (await _intranetMemberService.GetCurrentMemberAsync()).Id;
            var links = (await _myLinksService.GetManyAsync(memberId))
                .OrderByDescending(link => link.CreatedDate)
                .ToList();

            var contents = _nodeModelService.GetByIds(links.Select(el => el.ContentId));

            var models = links.Join(contents,
                link => link.ContentId,
                content => content.Id,
                (link, content) => new MyLinkItemModel
                {
                    Id = link.Id,
                    ActivityId=link.ActivityId,
                    ContentId=content.Id,
                    Name = link.ActivityId.HasValue ? GetLinkName(link.ActivityId.Value) : GetNavigationName(content),
                    Url = GetUrl(link, content)
                });

            return models;
        }

        public bool IsActivityLink(int contentId)
        {
            var page = _nodeModelService.Get(contentId);
            foreach (var type in _activityTypeProvider.All)
            {
                if (page.ContentTypeAlias.Equals(_documentTypeAliasProvider.GetDetailsPage(type)) ||
                    page.ContentTypeAlias.Equals(_documentTypeAliasProvider.GetEditPage(type)))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsGroupPage(int contentId)
        {
            var page = _nodeModelService.Get(contentId);

            return page.ContentTypeAlias.Equals(_documentTypeAliasProvider.GetGroupRoomPage());
        }

        private string GetLinkName(Guid entityId)
        {
            var linkName = GetGroupLink(entityId);
            if (linkName.IsNullOrEmpty())
            {
                return GetActivityLink(entityId);
            }

            return linkName;
        }

        private async Task<string> GetLinkNameAsync(Guid entityId)
        {
            var linkName = await GetGroupLinkAsync(entityId);
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

            if (activity.Type is IntranetActivityTypeEnum.Social)
            {
                var lengthForPreview = _navigationApplicationSettings.MyLinksActivityTitleLength;
                var description = activity.Description.StripHtml();
                return description.Length > lengthForPreview ? description.Substring(0, lengthForPreview) + "..." : description;
            }

            return activity.Title;
        }

        private static string GetUrl(MyLink link, INodeModel content)
        {
            if (link.QueryString.IsNullOrEmpty())
            {
                return content.Url;
            }

            return $"{content.Url}?{link.QueryString}";
        }

        private string GetGroupLink(Guid entityId)
        {
            var groupModel = _groupService.Get(entityId);
            return groupModel?.Title;
        }

        private async Task<string> GetGroupLinkAsync(Guid entityId)
        {
            var groupModel = await _groupService.GetAsync(entityId);
            return groupModel?.Title;
        }

        protected virtual string GetNavigationName(INodeModel content)
        {
            if(content is IUintraNavigationComposition navigationModel)
            {
                if (navigationModel.Navigation.NavigationTitle.Value.HasValue())
                {
                    return navigationModel.Navigation.NavigationTitle.Value;
                }
            }

            return content.Name;
        }
    }
}