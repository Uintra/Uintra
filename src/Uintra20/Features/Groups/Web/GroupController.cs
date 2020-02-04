using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using Compent.CommandBus;
using UBaseline.Core.Controllers;
using UBaseline.Core.Media;
using Uintra20.Attributes;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Links;
using Uintra20.Features.Media;
using Uintra20.Infrastructure.Constants;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.Providers;
using Umbraco.Core.Models.PublishedContent;

namespace Uintra20.Features.Groups.Web
{
    [ValidateModel]
    public class GroupController : UBaselineApiController
    {
        private const int ItemsPerPage = 10;

        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly IGroupService _groupService;
        private readonly IGroupMemberService _groupMemberService;
        private readonly IMediaHelper _mediaHelper;
        private readonly IIntranetMemberService<IGroupMember> _memberService;
        private readonly IGroupMediaService _groupMediaService;
        private readonly IProfileLinkProvider _profileLinkProvider;
        private readonly IImageHelper _imageHelper;
        private readonly ICommandPublisher _commandPublisher;
        private readonly IMediaModelService _mediaModelService;

        public GroupController(
            IGroupService groupService,
            IGroupMemberService groupMemberService,
            IMediaHelper mediaHelper,
            IGroupMediaService groupMediaService,
            IIntranetMemberService<IGroupMember> intranetMemberService,
            IProfileLinkProvider profileLinkProvider,
            IDocumentTypeAliasProvider documentTypeAliasProvider,
            IImageHelper imageHelper,
            ICommandPublisher commandPublisher,
            IMediaModelService mediaModelService)
        {
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _groupService = groupService;
            _groupMemberService = groupMemberService;
            _mediaHelper = mediaHelper;
            _memberService = intranetMemberService;
            _groupMediaService = groupMediaService;
            _profileLinkProvider = profileLinkProvider;
            _imageHelper = imageHelper;
            _commandPublisher = commandPublisher;
            _mediaModelService = mediaModelService;
        }

        public GroupLeftNavigationMenuViewModel LeftNavigation()
        {
            var groupPageXpath = XPathHelper.GetXpath(_documentTypeAliasProvider.GetHomePage(), _documentTypeAliasProvider.GetGroupOverviewPage());
            var groupPage = _umbracoHelper.TypedContentSingleAtXPath(groupPageXpath);

            var isPageActive = GetIsPageActiveFunc(_umbracoHelper.AssignedContentItem);

            var menuItems = GetMenuItems(groupPage);

            var result = new GroupLeftNavigationMenuViewModel
            {
                Items = menuItems,
                GroupOverviewPageUrl = groupPage.Url,
                IsActive = isPageActive(groupPage)
            };

            return result;
        }

        [HttpPost]
        public GroupModel Edit(GroupEditModel model)
        {
            var group = _groupService.Get(model.Id);
            group = Mapper.Map(model, group);
            group.ImageId = model.Media?.Split(',').First().ToNullableInt();
            var createdMedias = _mediaHelper.CreateMedia(model).ToList();
            if (createdMedias.Any())
            {
                group.ImageId = createdMedias.First();
            }
            _groupService.Edit(group);
            _groupMediaService.GroupTitleChanged(group.Id, group.Title);
            return _groupService.Get(model.Id);
        }

        [HttpPost]
        public GroupModel Create(GroupCreateModel createModel)
        {
            var groupId = _groupMemberService.Create(createModel);

            return _groupService.Get(groupId); ;
        }

        [HttpGet]
        public virtual IEnumerable<GroupViewModel> List(bool isMyGroupsPage = false, int page = 1)
        {
            IEnumerable<GroupViewModel> model = GetListModel(isMyGroupsPage, page);
            return model;
        }

        [HttpGet]
        public GroupInfoViewModel Info(Guid groupId)
        {
            var group = _groupService.Get(groupId);

            var groupInfo = group.Map<GroupInfoViewModel>();
            var currentMember = _memberService.GetCurrentMember();
            groupInfo.IsMember = _groupMemberService.IsGroupMember(group.Id, currentMember.Id);
            groupInfo.CanUnsubscribe = group.CreatorId != currentMember.Id;

            groupInfo.Creator = _memberService.Get(group.CreatorId).Map<MemberViewModel>();
            groupInfo.MembersCount = _groupMemberService.GetMembersCount(group.Id);
            groupInfo.CreatorProfileUrl = _profileLinkProvider.GetProfileLink(group.CreatorId).OriginalUrl;

            if (group.ImageId.HasValue)
            {
                groupInfo.GroupImageUrl = _imageHelper.GetImageWithPreset(_mediaModelService.Get(group.ImageId.Value).Url, UmbracoAliases.ImagePresets.GroupImageThumbnail);
            }

            return groupInfo;
        }

        [HttpPost]
        public virtual ActionResult Hide(Guid id)
        {
            var canHide = _groupService.CanHide(id);
            if (canHide)
            {
                var command = new HideGroupCommand(id);
                _commandPublisher.Publish(command);
            }

            return Json(canHide ? _groupLinkProvider.GetGroupsOverviewLink() :
                _groupLinkProvider.GetGroupLink(id));
        }

        [HttpPost]
        public virtual oRedirectToUmbracoPageResult Subscribe(Guid groupId)
        {
            var currentMember = _memberService.GetCurrentMember();

            if (_groupMemberService.IsGroupMember(groupId, currentMember.Id))
            {
                _groupMemberService.Remove(groupId, currentMember.Id);
            }
            else
            {
                var subscription = new GroupMemberSubscriptionModel
                {
                    MemberId = currentMember.Id,
                    IsAdmin = false
                };

                _groupMemberService.Add(groupId, subscription);
            }

            return oRedirectToCurrentUmbracoPage(Request.QueryString);
        }

        private IEnumerable<GroupLeftNavigationItemViewModel> GetMenuItems(IPublishedContent rootGroupPage)
        {
            var isPageActive = GetIsPageActiveFunc(_umbracoHelper.AssignedContentItem);

            var groupPageChildren = rootGroupPage.Children.Where(el => el.IsShowPageInSubNavigation()).ToList();

            foreach (var subPage in groupPageChildren)
            {
                if (subPage.IsShowPageInSubNavigation())
                {
                    if (_groupService.ValidatePermission(subPage))
                    {
                        yield return MapToLeftNavigationItem(subPage, isPageActive);
                    }
                }
                else
                {
                    yield return MapToLeftNavigationItem(subPage, isPageActive);
                }
            }
        }

        private IEnumerable<GroupViewModel> GetListModel(bool isMyGroupsPage, int page)
        {
            var take = page * ItemsPerPage;
            var currentMember = _memberService.GetCurrentMember();

            var allGroups = isMyGroupsPage
                ? _groupService.GetMany(currentMember.GroupIds).ToList()
                : _groupService.GetAllNotHidden().ToList();

            bool IsCurrentMemberInGroup(GroupModel g) => isMyGroupsPage || _groupMemberService.IsGroupMember(g.Id, currentMember.Id);

            var groups = allGroups
                .Select(g => MapGroupViewModel(g, IsCurrentMemberInGroup(g)))
                .OrderByDescending(g => g.Creator.Id == currentMember.Id)
                .ThenByDescending(s => s.IsMember)
                .ThenBy(g => g.Title);

            return groups;
        }
        private GroupViewModel MapGroupViewModel(GroupModel group, bool isCurrentUserMember)
        {
            var groupModel = group.Map<GroupViewModel>();
            groupModel.IsMember = isCurrentUserMember;
            groupModel.MembersCount = _groupMemberService.GetMembersCount(group.Id);
            groupModel.Creator = _memberService.Get(group.CreatorId).Map<MemberViewModel>();
            //groupModel.GroupUrl = _groupLinkProvider.GetGroupLink(group.Id);//TODO: Research
            if (groupModel.HasImage)
            {
                groupModel.GroupImageUrl = _imageHelper.GetImageWithPreset(_mediaModelService.Get(group.ImageId.Value).Url, UmbracoAliases.ImagePresets.GroupImageThumbnail);
            }

            return groupModel;
        }


        private static Func<IPublishedContent, bool> GetIsPageActiveFunc(IPublishedContent currentPage)
        {
            return p => currentPage.Id == p.Id;
        }

        private static GroupLeftNavigationItemViewModel MapToLeftNavigationItem(IPublishedContent page, Func<IPublishedContent, bool> isPageActive)
        {
            return new GroupLeftNavigationItemViewModel
            {
                Name = page.GetNavigationName(),
                Url = page.Url,
                IsActive = isPageActive(page)
            };
        }
    }
}