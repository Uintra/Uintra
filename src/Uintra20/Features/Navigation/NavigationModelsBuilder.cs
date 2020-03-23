using Compent.Extensions;
using System.Collections.Generic;
using System.Linq;
using UBaseline.Core.Navigation;
using UBaseline.Core.Node;
using UBaseline.Core.RequestContext;
using UBaseline.Shared.Navigation;
using UBaseline.Shared.Node;
using Uintra20.Core.HomePage;
using Uintra20.Core.Localization;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Core.User;
using Uintra20.Features.Navigation.Models;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.UintraInformation;

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
        private readonly IIntranetLocalizationService _intranetLocalizationService;
        public NavigationModelsBuilder(
            IUintraInformationService uintraInformationService,
            INodeModelService nodeModelService,
            INodeDirectAccessValidator nodeDirectAccessValidator,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            INavigationBuilder navigationBuilder,
            IIntranetUserContentProvider intranetUserContentProvider,
            IUBaselineRequestContext uBaselineRequestContext, 
            IIntranetLocalizationService intranetLocalizationService)
        {
            _uintraInformationService = uintraInformationService;
            _nodeModelService = nodeModelService;
            _nodeDirectAccessValidator = nodeDirectAccessValidator;
            _navigationBuilder = navigationBuilder;
            _intranetUserContentProvider = intranetUserContentProvider;
            _uBaselineRequestContext = uBaselineRequestContext;
            _intranetLocalizationService = intranetLocalizationService;
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

        public virtual TopNavigationModel GetMobileNavigation()
        {
            var model = new TopNavigationModel
            {
                CurrentMember = _intranetMemberService.GetCurrentMember(),
                Items = new List<TopNavigationItem>
                {
                    new TopNavigationItem
                    {
                        Name = _intranetLocalizationService.Translate("TopNavigation.Logout.lbl"),
                        Type = TopNavigationItemTypes.Logout,
                        Url = "/api/auth/logout".ToLinkModel()
                    }
                }
            };

            return model;
        }

        public virtual TopNavigationModel GetTopNavigationModel()
        {
            var menuItems = new List<TopNavigationItem>();
            var currentMember = _intranetMemberService.GetCurrentMember();

            if (currentMember.RelatedUser != null)
            {
                menuItems.Add(new TopNavigationItem
                {
                    Name = _intranetLocalizationService.Translate("TopNavigation.LoginToUmbraco.lbl"),
                    Type = TopNavigationItemTypes.LoginToUmbraco,
                    Url = "/api/auth/login/umbraco".ToLinkModel()
                });
            }
            menuItems.Add(new TopNavigationItem
            {
                Name = _intranetLocalizationService.Translate("TopNavigation.EditProfile.lbl"),
                Type = TopNavigationItemTypes.EditProfile,
                Url = _intranetUserContentProvider.GetEditPage().Url.ToLinkModel(),
            });

            menuItems.Add(new TopNavigationItem
            {
                Name = $"{_intranetLocalizationService.Translate("TopNavigation.UintraDocumentationLink.lnk")} v{_uintraInformationService.Version}",
                Type = TopNavigationItemTypes.UintraHelp,
                Url = _uintraInformationService.DocumentationLink.ToString().ToLinkModel()
            });

            menuItems.Add(new TopNavigationItem
            {
                Name = _intranetLocalizationService.Translate("TopNavigation.Logout.lbl"),
                Type = TopNavigationItemTypes.Logout,
                Url = "/api/auth/logout".ToLinkModel()
            });

            var model = new TopNavigationModel
            {
                CurrentMember = _intranetMemberService.GetCurrentMember(),
                Items = menuItems
            };

            return model;
        }

        public virtual IEnumerable<BreadcrumbItemViewModel> GetBreadcrumbsItems()
        {

            var pathToRoot = PathToRoot(_uBaselineRequestContext.Node).Reverse().ToList();
            var result = pathToRoot.Select(page =>
            {
                var navigationName = string.Empty;
                if (page is INavigationComposition composition)
                {
                    navigationName = composition.Navigation.NavigationTitle;
                }

                return new BreadcrumbItemViewModel
                {
                    Name = navigationName.HasValue() ? navigationName : page.Name,
                    Url = page.Url,
                    IsClickable = _uBaselineRequestContext.Node.Url != page.Url && !(page is HeadingPageModel)
                };
            });
            return result;
        }

        protected virtual IEnumerable<INodeModel> PathToRoot(INodeModel node)
        {
            var current = node;

            while (current != null)
            {
                yield return current;
                current = _nodeModelService.Get(current.ParentId);
            }
        }

        protected virtual bool IsActive(int nodeId)
        {
            return _uBaselineRequestContext.Node != null &&
                   (_uBaselineRequestContext.Node.Id == nodeId ||
                    _uBaselineRequestContext.Node.ParentIds.HasValue(i => i == nodeId));
        }
    }
}