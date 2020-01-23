using System.Collections.Generic;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Navigation.Models;
using Uintra20.Infrastructure;

namespace Uintra20.Features.Navigation
{
    public class NavigationModelsBuilder : INavigationModelsBuilder
    {
        private readonly IUintraInformationService _uintraInformationService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;

        public NavigationModelsBuilder(
            IUintraInformationService uintraInformationService,
            IIntranetMemberService<IntranetMember> intranetMemberService
            )
        {
            _uintraInformationService = uintraInformationService;
            _intranetMemberService = intranetMemberService;
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
                CurrentMember = _intranetMemberService.GetCurrentMember(),
                Items = menuItems
            };

            return model;
        }

    }
}