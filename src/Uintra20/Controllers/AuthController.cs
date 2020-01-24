using System;
using System.Collections.Generic;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Security;
using Uintra20.Core.Authentication;
using Uintra20.Core.Authentication.Models;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Helpers;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Notification;
using Uintra20.Features.Notification.Configuration;
using Uintra20.Features.Notification.Entities.Base;
using Uintra20.Features.Notification.Services;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.Providers;
using Uintra20.Models.UmbracoIdentity;
using Umbraco.Core.Services;
using Umbraco.Web;
using UmbracoIdentity;

namespace Uintra20.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        protected virtual string DefaultRedirectUrl { get; } = "/";
        protected virtual string UmbracoRedirectUrl { get; } = "/umbraco";

        private readonly UmbracoMembersUserManager<UmbracoApplicationMember> _userManager;
        private readonly IAuthenticationService _authenticationService;
        private readonly IClientTimezoneProvider _clientTimezoneProvider;
        private readonly IMemberServiceHelper _memberServiceHelper;
        private readonly ICacheableIntranetMemberService _cacheableIntranetMemberService;
        private readonly IMemberService _memberService;
        private readonly INotificationsService _notificationsService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly UmbracoContext _umbracoContext;

        public AuthController(
            UmbracoMembersUserManager<UmbracoApplicationMember> userManager,
            IAuthenticationService authenticationService,
            IClientTimezoneProvider clientTimezoneProvider,
            IMemberServiceHelper memberServiceHelper,
            ICacheableIntranetMemberService cacheableIntranetMemberService,
            IMemberService memberService,
            INotificationsService notificationsService,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            UmbracoContext umbracoContext)
        {
            _userManager = userManager;
            _authenticationService = authenticationService;
            _clientTimezoneProvider = clientTimezoneProvider;
            _memberServiceHelper = memberServiceHelper;
            _cacheableIntranetMemberService = cacheableIntranetMemberService;
            _memberService = memberService;
            _notificationsService = notificationsService;
            _intranetMemberService = intranetMemberService;
            _umbracoContext = umbracoContext;
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
                return BadRequest("Credentials not valid");

            await _authenticationService.LoginAsync(login, loginModel.Password);
            _clientTimezoneProvider.SetClientTimezone(loginModel.ClientTimezoneId);

            var member = _memberService.GetByUsername(login);
            if (!_memberServiceHelper.IsFirstLoginPerformed(member))
            {
                SendWelcomeNotification(member.Key);
                _memberServiceHelper.SetFirstLoginPerformed(member);
            }

            return Ok();
        }

        [HttpPost]
        [Route("login/umbraco")]
        public IHttpActionResult LoginToUmbraco()
        {
            var currentMember = _intranetMemberService.GetCurrentMember();
            var relatedUser = currentMember.RelatedUser;
            if (!relatedUser.IsValid)
                return Redirect(DefaultRedirectUrl);
            _umbracoContext.Security.PerformLogin(relatedUser.Id);
            return Redirect(UmbracoRedirectUrl);
        }

        [HttpPost]
        [Route("logout")]
        public IHttpActionResult Logout()
        {
            _authenticationService.Logout();

            return Ok();
        }

        private Task SetDefaultUserData()
        {
            var mbr = _memberService.GetByEmail(UsersInstallationConstants.DefaultMember.Email);

            if (mbr == null || _memberServiceHelper.IsFirstLoginPerformed(mbr))
                return Task.CompletedTask;

            _memberService.SavePassword(mbr, UsersInstallationConstants.DefaultMember.Password);
            _memberService.AssignRole(mbr.Id, UsersInstallationConstants.MemberGroups.GroupWebMaster);

            return _cacheableIntranetMemberService.UpdateMemberCacheAsync(mbr.Key);
        }

        private void SendWelcomeNotification(Guid userId)
        {
            _notificationsService.ProcessNotification(new NotifierData
            {
                NotificationType = NotificationTypeEnum.Welcome,
                ReceiverIds = new List<Guid> { userId },
                ActivityType = CommunicationTypeEnum.Member
            });
        }
    }
}