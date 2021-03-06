﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using UBaseline.Core.Controllers;
using UBaseline.Core.Navigation;
using UBaseline.Core.Node;
using UBaseline.Core.RequestContext;
using Uintra.Core.HomePage;
using Uintra.Core.Member.Entities;
using Uintra.Core.Member.Services;
using Uintra.Features.Breadcrumbs.Models;
using Uintra.Features.Breadcrumbs.Services.Contracts;
using Uintra.Features.Groups.Helpers;
using Uintra.Features.Links.Models;
using Uintra.Features.Navigation.Builders;
using Uintra.Features.Navigation.Helpers;
using Uintra.Features.Navigation.Models;
using Uintra.Features.Navigation.Models.MyLinks;
using Uintra.Features.Notification;
using Uintra.Features.Notification.Services;
using Uintra.Features.Notification.ViewModel;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Features.Navigation.Web
{
    public class IntranetNavigationController : UBaselineApiController
    {
        private readonly INavigationModelsBuilder _navigationModelsBuilder;
        private readonly INodeModelService _nodeModelService;
        private readonly IMyLinksHelper _myLinksHelper;
        private readonly IGroupHelper _groupHelper;
        private readonly IUBaselineRequestContext _ubaselineRequestContext;
        private readonly IPopupNotificationService _popupNotificationService;
        private readonly IBreadcrumbService _breadcrumbService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;

        public IntranetNavigationController(
            INavigationModelsBuilder navigationModelsBuilder,
            INodeModelService nodeModelService,
            IMyLinksHelper myLinksHelper,
            IGroupHelper groupHelper,
            IUBaselineRequestContext ubaselineRequestContext,
            IPopupNotificationService popupNotificationService,
            IBreadcrumbService breadcrumbService,
            IIntranetMemberService<IntranetMember> intranetMemberService)
        {
            _navigationModelsBuilder = navigationModelsBuilder;
            _nodeModelService = nodeModelService;
            _myLinksHelper = myLinksHelper;
            _groupHelper = groupHelper;
            _ubaselineRequestContext = ubaselineRequestContext;
            _popupNotificationService = popupNotificationService;
            _breadcrumbService = breadcrumbService;
            _intranetMemberService = intranetMemberService;
        }

        [HttpGet]
        public virtual TopNavigationViewModel MobileNavigation()
        {
            var model = _navigationModelsBuilder.GetMobileNavigation();
            var viewModel = model.Map<TopNavigationViewModel>();
            return viewModel;
        }

        [HttpGet]
        public virtual TopNavigationViewModel TopNavigation()
        {
            var model = _navigationModelsBuilder.GetTopNavigationModel();
            var viewModel = model.Map<TopNavigationViewModel>();
            viewModel.CurrentMember = model.CurrentMember.ToViewModel();

            ShowPopupNotification();
            return viewModel;
        }

        private void ShowPopupNotification()
        {
            var currentMemberId = _intranetMemberService.GetCurrentMemberId();
            var notifications = _popupNotificationService.Get(currentMemberId)
                .Map<IEnumerable<PopupNotificationViewModel>>();
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<UintraHub>();
            if (notifications.Any())
            {
                Task.Run(() =>
                {
                    Thread.Sleep(5000);
                    hubContext.Clients.User(currentMemberId.ToString()).showPopup(notifications);
                });
            }
        }

        [HttpGet]
        public virtual UintraLinkModel UserList()
        {
            var homeModel = (HomePageModel) _ubaselineRequestContext.HomeNode;
            if (homeModel.UserListPage.Value.HasValue)
            {
                var userListPage = _nodeModelService.Get(homeModel.UserListPage.Value.Value);
                return userListPage.Url.ToLinkModel();
            }
            return null;
        }

        [HttpGet]
        public virtual async Task<LeftNavigationViewModel> LeftNavigation()
        {
            var result = new LeftNavigationViewModel
            {
                MenuItems = GetMenuItems(),
                GroupItems = _groupHelper.GroupNavigation(),
                MyLinks = await GetMyLinksAsync(),
                SharedLinks = GetSharedLinks()
            };

            return result;
        }

        [HttpGet]
        public virtual IEnumerable<BreadcrumbViewModel> Breadcrumbs() => 
            _breadcrumbService.GetBreadcrumbs();

        private IEnumerable<SharedLinkApiViewModel> GetSharedLinks()
        {
            var sharedLinks = _nodeModelService.AsEnumerable().OfType<SharedLinkItemModel>()
                .Where(sl => sl.Links.Value != null);

            sharedLinks = sharedLinks.ToArray();

            var result = sharedLinks.OrderBy(sl => sl.Sort.Value).Select(MapSharedLinkItemModel);
            return result;
        }

        private IEnumerable<MenuItemViewModel> GetMenuItems()
        {
            return _navigationModelsBuilder.GetLeftSideNavigation().Select(MapMenuItem);
        }

        protected virtual async Task<IEnumerable<MyLinkItemViewModel>> GetMyLinksAsync()
        {
            var linkModels = await _myLinksHelper.GetMenuAsync();

            return linkModels.Map<IEnumerable<MyLinkItemViewModel>>();
        }

        protected virtual SharedLinkApiViewModel MapSharedLinkItemModel(SharedLinkItemModel model)
        {
            return new SharedLinkApiViewModel
            {
                LinksGroupTitle = model.LinksGroupTitle,
                Links = model.Links.Value.Select(sharedLink => sharedLink.ToViewModel())
            };
        }

        protected virtual MenuItemViewModel MapMenuItem(TreeNavigationItemModel model)
        {
            var item = _nodeModelService.Get(model.Id);

            var result = model.Map<MenuItemViewModel>();
            result.IsHeading = item is HeadingPageModel;
            result.IsHomePage = item is HomePageModel;
            result.IsClickable = !result.IsHeading && result.IsActive;

            return result;
        }
    }
}