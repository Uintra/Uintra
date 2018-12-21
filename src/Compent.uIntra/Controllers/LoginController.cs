using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Compent.uIntra.Core.Login.Models;
using Compent.Uintra.Core.Login.Models;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants;
using Google.Apis.Auth;
using Localization.Umbraco.Attributes;
using Uintra.Core;
using Uintra.Core.ApplicationSettings;
using Uintra.Core.Localization;
using Uintra.Notification;
using Uintra.Notification.Base;
using Uintra.Notification.Configuration;
using Uintra.Users;
using Uintra.Users.Web;
using Umbraco.Core.Services;
using static LanguageExt.Prelude;

namespace Compent.Uintra.Controllers
{
    [AllowAnonymous]
    [ThreadCulture]
    public class LoginController : LoginControllerBase
    {
        private readonly ITimezoneOffsetProvider _timezoneOffsetProvider;
        private readonly IIntranetLocalizationService _intranetLocalizationService;
        private readonly INotificationsService _notificationsService;
        private readonly IMemberServiceHelper _memberServiceHelper;
        private readonly IMemberService _memberService;
        private readonly ICacheableIntranetUserService _cacheableIntranetUserService;
        private readonly IUintraInformationService uintraInformationService;

        protected override string LoginViewPath => "~/Views/Login/Login.cshtml";

        public LoginController(
            ITimezoneOffsetProvider timezoneOffsetProvider,
            IIntranetLocalizationService intranetLocalizationService,
            INotificationsService notificationsService,
            IMemberServiceHelper memberServiceHelper,
            IMemberService memberService,
            ICacheableIntranetUserService cacheableIntranetUserService,
            IApplicationSettings applicationSettings,
            IUintraInformationService uintraInformationService)
            : base(timezoneOffsetProvider, intranetLocalizationService, applicationSettings)
        {
            _timezoneOffsetProvider = timezoneOffsetProvider;
            _intranetLocalizationService = intranetLocalizationService;
            _notificationsService = notificationsService;
            _memberServiceHelper = memberServiceHelper;
            _memberService = memberService;
            _cacheableIntranetUserService = cacheableIntranetUserService;
            this.uintraInformationService = uintraInformationService;
        }

        [HttpPost]
        public async Task<JsonResult> IdTokenVerification(string idToken, int clientTimezoneOffset)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken,
                new GoogleJsonWebSignature.ValidationSettings()
                {
                    IssuedAtClockTolerance = TimeSpan.FromDays(1) // for cases when server's time different from UTC time (google time).
                });
            if (payload != null) {
                var member = _memberService.GetByEmail(payload.Email);
                if (member != null)
                {
                    FormsAuthentication.SetAuthCookie(member.Username, true);
                    _timezoneOffsetProvider.SetTimezoneOffset(clientTimezoneOffset);

                    if (!_memberServiceHelper.IsFirstLoginPerformed(member))
                    {
                        SendWelcomeNotification(member.Key);
                        _memberServiceHelper.SetFirstLoginPerformed(member);
                    }

                    return Json(new GoogleAuthResultModel()
                    {
                        Url = DefaultRedirectUrl,
                        Success = true
                    });
                }
            }
            return Json(new GoogleAuthResultModel());
        }

        [HttpGet]
        public ActionResult LoginUintra()
        {
            if(Members.GetCurrentLoginStatus().IsLoggedIn)
            {
                return Redirect(DefaultRedirectUrl);
            }

            var model = new LoginModel()
            {
                GoogleSettings = GetGoogleSettings(),
                CurrentIntranetVersion = uintraInformationService.Version
            };
            return View(LoginViewPath, model);
        }        

        [HttpPost]
        public ActionResult LoginUintra(LoginModel model)
        {
            model.GoogleSettings = GetGoogleSettings();
            if (!ModelState.IsValid)
            {
                return View(LoginViewPath, model);
            }

            SetDefaultUserData();

            if (!Membership.ValidateUser(model.Login, model.Password))
            {
                ModelState.AddModelError("UserValidation", _intranetLocalizationService.Translate("Login.Validation.UserNotValid"));
                return View(LoginViewPath, model);
            }

            var redirectUrl = model.ReturnUrl ?? DefaultRedirectUrl;

            if (Members.Login(model.Login, model.Password))
            {
                _timezoneOffsetProvider.SetTimezoneOffset(model.ClientTimezoneOffset);

                var member = _memberService.GetByUsername(model.Login);
                if (!_memberServiceHelper.IsFirstLoginPerformed(member))
                {
                    SendWelcomeNotification(member.Key);
                    _memberServiceHelper.SetFirstLoginPerformed(member);
                }
            }

            return Redirect(redirectUrl);
        }

       

        private void SetDefaultUserData()
        {
            var mbr = _memberService.GetByEmail(UsersInstallationConstants.DefaultMember.Email);

            if (mbr != null && !_memberServiceHelper.IsFirstLoginPerformed(mbr))
            {
                _memberService.SavePassword(mbr, UsersInstallationConstants.DefaultMember.Password);
                _memberService.AssignRole(mbr.Id, UsersInstallationConstants.MemberGroups.GroupWebMaster);
                _cacheableIntranetUserService.UpdateUserCache(mbr.Key);
            }
        }

        private void SendWelcomeNotification(Guid userId)
        {
            _notificationsService.ProcessNotification(new NotifierData
            {
                NotificationType = NotificationTypeEnum.Welcome,
                ReceiverIds = List(userId),
                ActivityType = CommunicationTypeEnum.Member
            });
        }
    }
}