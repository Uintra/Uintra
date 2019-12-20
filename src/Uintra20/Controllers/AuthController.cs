using System.Web.Http;
using System.Web.Security;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants;
using Uintra20.Core.Authentication;
using Uintra20.Core.Authentication.Models;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Core.Member.Helpers;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.Providers;
using Umbraco.Core.Services;

namespace Uintra20.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IClientTimezoneProvider _clientTimezoneProvider;
        private readonly IMemberServiceHelper _memberServiceHelper;
        private readonly ICacheableIntranetMemberService _cacheableIntranetMemberService;
        private readonly IMemberService _memberService;

        public AuthController(
            IAuthenticationService authenticationService,
            IClientTimezoneProvider clientTimezoneProvider,
            IMemberServiceHelper memberServiceHelper,
            ICacheableIntranetMemberService cacheableIntranetMemberService,
            IMemberService memberService)
        {
            this._authenticationService = authenticationService;
            this._clientTimezoneProvider = clientTimezoneProvider;
            _memberServiceHelper = memberServiceHelper;
            _cacheableIntranetMemberService = cacheableIntranetMemberService;
            _memberService = memberService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public IHttpActionResult Login(LoginModelBase loginModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.CollectErrors());

            if (!Membership.ValidateUser(loginModel.Login, loginModel.Password))
                return BadRequest("Credentials not valid");
            
            _authenticationService.Login(loginModel.Login, loginModel.Password);
            _clientTimezoneProvider.SetClientTimezone(loginModel.ClientTimezoneId);

            var member = _memberService.GetByUsername(loginModel.Login);
            if (!_memberServiceHelper.IsFirstLoginPerformed(member))
            {
				//todo uncomment when notifications will be ready
	            //SendWelcomeNotification(member.Key);
	            _memberServiceHelper.SetFirstLoginPerformed(member);
            }

			return Ok();
        }

        [HttpPost]
        [Route("logout")]
        public IHttpActionResult Logout()
        {
	        _authenticationService.Logout();

	        return Ok();
        }

        private void SetDefaultUserData()
        {
	        var mbr = _memberService.GetByEmail(UsersInstallationConstants.DefaultMember.Email);

	        if (mbr != null && !_memberServiceHelper.IsFirstLoginPerformed(mbr))
	        {
		        _memberService.SavePassword(mbr, UsersInstallationConstants.DefaultMember.Password);
		        _memberService.AssignRole(mbr.Id, UsersInstallationConstants.MemberGroups.GroupWebMaster);
		        _cacheableIntranetMemberService.UpdateMemberCache(mbr.Key);
	        }
        }

        //private void SendWelcomeNotification(Guid userId)
        //{
	       // _notificationService.ProcessNotification(new NotifierData
	       // {
		      //  NotificationType = NotificationTypeEnum.Welcome,
		      //  ReceiverIds = List(userId),
		      //  ActivityType = CommunicationTypeEnum.Member
	       // });
        //}
	}
}