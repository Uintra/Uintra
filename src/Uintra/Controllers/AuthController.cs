using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Security;
using Uintra.Core.Authentication;
using Uintra.Core.Authentication.Models;
using Uintra.Core.Localization;
using Uintra.Core.Member.Entities;
using Uintra.Core.Member.Helpers;
using Uintra.Core.Member.Services;
using Uintra.Features.Notification;
using Uintra.Features.Notification.Configuration;
using Uintra.Features.Notification.Entities.Base;
using Uintra.Features.Notification.Services;
using Uintra.Infrastructure.Extensions;
using Uintra.Infrastructure.Providers;
using Uintra.Models.UmbracoIdentity;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using UmbracoIdentity;

namespace Uintra.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        private readonly UmbracoMembersUserManager<UmbracoApplicationMember> _userManager;
        private readonly IAuthenticationService _authenticationService;
        private readonly IClientTimezoneProvider _clientTimezoneProvider;
        private readonly IMemberServiceHelper _memberServiceHelper;
        private readonly IMemberService _memberService;
        private readonly INotificationsService _notificationsService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly UmbracoContext _umbracoContext;
        private readonly IIntranetLocalizationService _intranetLocalizationService;

        public AuthController(
            UmbracoMembersUserManager<UmbracoApplicationMember> userManager,
            IAuthenticationService authenticationService,
            IClientTimezoneProvider clientTimezoneProvider,
            IMemberServiceHelper memberServiceHelper,
            IMemberService memberService,
            INotificationsService notificationsService,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            UmbracoContext umbracoContext,
            IIntranetLocalizationService intranetLocalizationService)
        {
            _userManager = userManager;
            _authenticationService = authenticationService;
            _clientTimezoneProvider = clientTimezoneProvider;
            _memberServiceHelper = memberServiceHelper;
            _memberService = memberService;
            _notificationsService = notificationsService;
            _intranetMemberService = intranetMemberService;
            _umbracoContext = umbracoContext;
            _intranetLocalizationService = intranetLocalizationService; 
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IHttpActionResult> Login(LoginModelBase loginModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.CollectErrors());

            var user = await _userManager.FindByEmailAsync(loginModel.Login);
            var login = user != null ? user.UserName : loginModel.Login;

            if (!Membership.ValidateUser(login, loginModel.Password))
                return BadRequest(_intranetLocalizationService.Translate("credentialsNotValid.lbl"));

            await _authenticationService.LoginAsync(login, loginModel.Password);
            _clientTimezoneProvider.SetClientTimezone(loginModel.ClientTimezoneId);

            var member = _memberService.GetByUsername(login);
            if (!_memberServiceHelper.IsFirstLoginPerformed(member))
            {
                GreetNewMember(member);
            }

            return Ok();
        }

        [HttpPost]
        [Route("login/umbraco")]
        public IHttpActionResult LoginToUmbraco()
        {
            var currentMember = _intranetMemberService.GetCurrentMember();
            var relatedUser = currentMember.RelatedUser;
            if (relatedUser == null || !relatedUser.IsValid)
                return Content(HttpStatusCode.Forbidden, "Member has no related user ");
            _umbracoContext.Security.PerformLogin(relatedUser.Id);
            return Ok();
        }

        [HttpPost]
        [Route("logout")]
        public IHttpActionResult Logout()
        {
            _authenticationService.Logout();

            return Ok();
        }

        private void GreetNewMember(IMember member)
        {
            _notificationsService.ProcessNotification(new NotifierData
            {
                NotificationType = NotificationTypeEnum.Welcome,
                ReceiverIds = new List<Guid> {member.Key},
                ActivityType = CommunicationTypeEnum.Member
            });

            _memberServiceHelper.SetFirstLoginPerformed(member);
        }
    }
}