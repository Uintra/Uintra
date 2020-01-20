using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uintra20.Features.Navigation.Models;
using Uintra20.Infrastructure;

namespace Uintra20.Features.Navigation
{
    public class NavigationModelsBuilder : INavigationModelsBuilder
    {
        private readonly IUintraInformationService _uintraInformationService;

        public NavigationModelsBuilder(IUintraInformationService uintraInformationService)
        {
            _uintraInformationService = uintraInformationService;
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