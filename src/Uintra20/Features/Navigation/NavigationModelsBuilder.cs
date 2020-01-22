using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Compent.Extensions;
using UBaseline.Core.Navigation;
using UBaseline.Core.Node;
using Uintra20.Features.Navigation.Models;
using Uintra20.Infrastructure;

namespace Uintra20.Features.Navigation
{
    public class NavigationModelsBuilder : INavigationModelsBuilder
    {
        private readonly IUintraInformationService _uintraInformationService;
        private readonly INodeModelService _nodeModelService;
        private readonly INodeDirectAccessValidator _nodeDirectAccessValidator;
        private readonly INavigationBuilder _navigationBuilder;

        public NavigationModelsBuilder(
            IUintraInformationService uintraInformationService,
            INodeModelService nodeModelService,
            INodeDirectAccessValidator nodeDirectAccessValidator,
            INavigationBuilder navigationBuilder)
        {
            _uintraInformationService = uintraInformationService;
            _nodeModelService = nodeModelService;
            _nodeDirectAccessValidator = nodeDirectAccessValidator;
            _navigationBuilder = navigationBuilder;
        }

        public virtual IEnumerable<TreeNavigationItemModel> GetLeftSideNavigation()
        {
            var test = _nodeModelService.AsEnumerable()
                .Where(i => i.Level >= 1 && _nodeDirectAccessValidator.HasAccess(i)).OfType<IUintraNavigationComposition>();
            var testNames = test.Select(x => x.Name).ToList();

            var navigationNodes = _nodeModelService.AsEnumerable()
                .Where(i => i.Level >= 1 && _nodeDirectAccessValidator.HasAccess(i))
                .OfType<IUintraNavigationComposition>()
                .OrderBy(i => i.SortOrder)
                .Where(i => i.Navigation.ShowInMenu.Value && i.Url.HasValue());

            IEnumerable<TreeNavigationItemModel> items = _navigationBuilder.GetTreeNavigation(navigationNodes);

            return items;
        }

        public virtual TopNavigationModel GetTopNavigationModel()
        {
            var menuItems = new List<TopNavigationItem>()
            {
                new TopNavigationItem()
                {
                    Name = "Login To Umbraco",
                    Type = TopNavigationItemTypes.LoginToUmbraco,
                    Url = "/umbraco/api/navigation/logintoumbraco"
                },
                new TopNavigationItem()
                {
                    Name = "Edit Profile",
                    Type = TopNavigationItemTypes.EditProfile,
                    Url = "/profile-edit"
                },
                new TopNavigationItem()
                {
                    Name = $"Uintra Help v{_uintraInformationService.Version}",
                    Type = TopNavigationItemTypes.UintraHelp,
                    Url = _uintraInformationService.DocumentationLink.ToString()
                },
                new TopNavigationItem()
                {
                    Name = "Logout",
                    Type = TopNavigationItemTypes.Logout,
                    Url = "/umbraco/api/navigation/logout"
                }
            };
            var model = new TopNavigationModel()
            {
                Items = menuItems
            };

            return model;
        }

    }
}