using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using UBaseline.Core.Controllers;
using Uintra20.Core.Authentication;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Navigation.Models;
using Uintra20.Infrastructure.Extensions;
using Umbraco.Web.WebApi;

namespace Uintra20.Features.Navigation.Web
{
    public class NavigationController : UmbracoApiController
    {
        protected virtual string DefaultRedirectUrl { get; } = "/";
        protected virtual string UmbracoRedirectUrl { get; } = "/umbraco";

        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly IAuthenticationService _authenticationService;
        private readonly INavigationModelsBuilder _navigationModelsBuilder;

        public NavigationController(
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            IAuthenticationService authenticationService,
            INavigationModelsBuilder navigationModelsBuilder)
        {
            _intranetMemberService = intranetMemberService;
            _authenticationService = authenticationService;
            _navigationModelsBuilder = navigationModelsBuilder;
        }

        public virtual TopNavigationViewModel TopNavigation()
        {
            var model = _navigationModelsBuilder.GetTopNavigationModel();
            var viewModel = model.Map<TopNavigationViewModel>();
            return viewModel;
        }

        public IHttpActionResult LoginToUmbraco()
        {
            var currentMember = _intranetMemberService.GetCurrentMember();
            var relatedUser = currentMember.RelatedUser;
            if (!relatedUser.IsValid)
                return Redirect(DefaultRedirectUrl);
            UmbracoContext.Security.PerformLogin(relatedUser.Id);
            return Redirect(UmbracoRedirectUrl);
        }

        public IHttpActionResult Logout()
        {
            _authenticationService.Logout();
            return Redirect(DefaultRedirectUrl);
        }
    }


}