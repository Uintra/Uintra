﻿using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.CentralFeed;
using uIntra.Core.Activity;
using uIntra.Core.Extentions;
using uIntra.Core.Grid;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using Umbraco.Core.Models;
using Umbraco.Web;
using static uIntra.Groups.Constants.GroupConstants;

namespace uIntra.Groups
{
    public class GroupFeedContentService : FeedContentServiceBase, IGroupFeedContentService
    {
        private readonly IGroupService _groupService;
        private readonly IGroupFeedLinkService _groupFeedLinkService;
        private readonly IGroupContentProvider _contentProvider;

        protected override string FeedPluginAlias { get; } = GroupFeedPluginAlias;
        protected override string ActivityCreatePluginAlias { get; } = GroupActivityCreatePluginAlias;

        public GroupFeedContentService(
            IGroupService groupService,
            IGroupFeedLinkService groupFeedLinkService,
            IGroupContentProvider contentProvider)
        {
            _groupService = groupService;
            _groupFeedLinkService = groupFeedLinkService;
            _contentProvider = contentProvider;
        }

        public ActivityFeedTabModel GetMainFeedTab(IPublishedContent currentContent, Guid groupId)
        {
            var groupRoom = _contentProvider.GetGroupRoomPage();
            var type = GetFeedTabType(groupRoom);
            var result = new ActivityFeedTabModel
            {
                Content = groupRoom,
                Type = type,
                IsActive = groupRoom.Id == currentContent.Id,
                Links = _groupFeedLinkService.GetCreateLinks(type, groupId)
            };
            return result;
        }

        public IEnumerable<ActivityFeedTabModel> GetActivityTabs(IPublishedContent currentContent, IIntranetUser user, Guid groupId)
        {
            yield return GetMainFeedTab(currentContent, groupId);

            foreach (var content in GetContent())
            {
                var tabType = GetFeedTabType(content);
                var activityType = tabType.Id.ToEnum<IntranetActivityTypeEnum>();

                if (activityType == null) continue;

                var tab = new ActivityFeedTabModel
                {
                    Content = content,
                    Type = tabType,
                    IsActive = content.IsAncestorOrSelf(currentContent),
                    Links = _groupFeedLinkService.GetCreateLinks(tabType, groupId)
                };

                yield return tab;
            }
        }

        public IEnumerable<PageTabModel> GetPageTabs(IPublishedContent currentContent, IIntranetUser user, Guid groupId)
        {
            Func<IPublishedContent, bool> skipPage = GetPageSkipResolver(user, groupId);

            foreach (var content in GetContent())
            {
                if (skipPage(content))
                    continue;
                var tabType = GetFeedTabType(content);
                var activityType = tabType.Id.ToEnum<IntranetActivityTypeEnum>();
                if (activityType == null)
                    yield return GetPageTab(currentContent, content, groupId);
            }
        }

        private Func<IPublishedContent, bool> GetPageSkipResolver(IIntranetUser user, Guid groupId)
        {
            var canEdit = _groupService.CanEdit(groupId, user);
            var editGroupPage = _contentProvider.GetEditPage();

            var deactivatedPage = _contentProvider.GetDeactivatedGroupPage();

            bool SkipPage(IPublishedContent content) => !canEdit && AreSamePages(editGroupPage, content) || AreSamePages(deactivatedPage, content);
            return SkipPage;
        }

        private PageTabModel GetPageTab(IPublishedContent currentContent, IPublishedContent content, Guid groupId)
        {
            return new PageTabModel
            {
                Content = content,
                IsActive = content.IsAncestorOrSelf(currentContent),
                Title = content.Name,
                Link = content.Url.AddGroupId(groupId)
            };
        }

        private static bool AreSamePages(IPublishedContent f, IPublishedContent s) => f.Id == s.Id;

        private IEnumerable<IPublishedContent> GetContent()
        {
            return _contentProvider.GetGroupRoomPage().Children;
        }
    }
}