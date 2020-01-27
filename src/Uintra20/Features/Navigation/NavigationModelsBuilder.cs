using System.Collections.Generic;
using System.Linq;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Compent.Extensions;
using UBaseline.Core.Navigation;
using UBaseline.Core.Node;
using Uintra20.Core.User;
using UBaseline.Core.RequestContext;
using Uintra20.Core.HomePage;
using Uintra20.Features.Navigation.Models;
using Uintra20.Infrastructure;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Navigation
{
    public class NavigationModelsBuilder : INavigationModelsBuilder
    {
        private readonly IUintraInformationService _uintraInformationService;
        private readonly INodeModelService _nodeModelService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly INodeDirectAccessValidator _nodeDirectAccessValidator;
        private readonly INavigationBuilder _navigationBuilder;
        private readonly IIntranetUserContentProvider _intranetUserContentProvider;
        private readonly IUBaselineRequestContext _uBaselineRequestContext;

        public NavigationModelsBuilder(
            IUintraInformationService uintraInformationService,
            INodeModelService nodeModelService,
            INodeDirectAccessValidator nodeDirectAccessValidator,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            INavigationBuilder navigationBuilder,
            IIntranetUserContentProvider intranetUserContentProvider,
            IUBaselineRequestContext uBaselineRequestContext)
        {
            _uintraInformationService = uintraInformationService;
            _nodeModelService = nodeModelService;
            _nodeDirectAccessValidator = nodeDirectAccessValidator;
            _navigationBuilder = navigationBuilder;
            _intranetUserContentProvider = intranetUserContentProvider;
            _uBaselineRequestContext = uBaselineRequestContext;
            _intranetMemberService = intranetMemberService;
        }

        public virtual IEnumerable<TreeNavigationItemModel> GetLeftSideNavigation()
        {
            var navigationNodes = _nodeModelService.AsEnumerable()
                .Where(i => i.Level >= 1 && _nodeDirectAccessValidator.HasAccess(i) && !(i is HomePageModel))
                .OfType<IUintraNavigationComposition>()
                .OrderBy(i => i.SortOrder)
                .Where(i => i.Navigation.ShowInMenu.Value && i.Url.HasValue());

            var items = _navigationBuilder.GetTreeNavigation(navigationNodes);

            var home = _nodeModelService.AsEnumerable().OfType<HomePageModel>().First();
            items = items.Prepend(new TreeNavigationItemModel
            {
                Id = home.Id,
                IsActive = IsActive(home.Id),
                Level = home.Level,
                ParentId = home.ParentId,
                SortOrder = home.SortOrder,
                Title = home.Navigation.NavigationTitle,
                Url = home.Url
            });

            return items;
        }

        public virtual TopNavigationModel GetTopNavigationModel()
        {
            var menuItems = new List<TopNavigationItem>();
            var currentMember = _intranetMemberService.GetCurrentMember();

            if (currentMember.RelatedUser != null)
            {
                menuItems.Add(new TopNavigationItem()
                {
                    Name = "Login To Umbraco",
                    Type = TopNavigationItemTypes.LoginToUmbraco,
                    Url = "/api/auth/login/umbraco".ToLinkModel()
                });
            }
            menuItems.Add(new TopNavigationItem()
            {
                Name = "Edit Profile",
                Type = TopNavigationItemTypes.EditProfile,
                Url = _intranetUserContentProvider.GetEditPage().Url.ToLinkModel(),
            });

            menuItems.Add(new TopNavigationItem()
            {
                Name = $"Uintra Help v{_uintraInformationService.Version}",
                Type = TopNavigationItemTypes.UintraHelp,
                Url = _uintraInformationService.DocumentationLink.ToString().ToLinkModel()
            });

            menuItems.Add(new TopNavigationItem()
            {
                Name = "Logout",
                Type = TopNavigationItemTypes.Logout,
                Url = "/api/auth/logout".ToLinkModel()
            });

            var model = new TopNavigationModel()
            {
                CurrentMember = _intranetMemberService.GetCurrentMember(),
                Items = menuItems
            };

            return model;
        }

        protected virtual bool IsActive(int nodeId)
        {
            return _uBaselineRequestContext.Node != null &&
                   (_uBaselineRequestContext.Node.Id == nodeId ||
                    _uBaselineRequestContext.Node.ParentIds.HasValue(i => i == nodeId));
        }
    }
}