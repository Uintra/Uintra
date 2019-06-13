using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Compent.uIntra.Core.Login.Models;
using Compent.Uintra.Core.Login.Models;
using Compent.Uintra.Core.Updater;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants;
using Compent.Uintra.Core.Updater.Migrations._1._3;
using Compent.Uintra.Core.Updater.Migrations._1._3.Steps;
using Google.Apis.Auth;
using Localization.Umbraco.Attributes;
using Uintra.Core;
using Uintra.Core.ApplicationSettings;
using Uintra.Core.Localization;
using Uintra.Core.MigrationHistories;
using Uintra.Notification;
using Uintra.Notification.Base;
using Uintra.Notification.Configuration;
using Uintra.Users;
using Uintra.Users.Web;
using Umbraco.Core;
using Umbraco.Core.Services;
using static LanguageExt.Prelude;

namespace Compent.Uintra.Controllers
{
    [AllowAnonymous]
    [ThreadCulture]
    public class LoginController : LoginControllerBase
    {
        private readonly IClientTimezoneProvider _clientTimezoneProvider;
        private readonly IIntranetLocalizationService _intranetLocalizationService;
        private readonly INotificationsService _notificationsService;
        private readonly IMemberServiceHelper _memberServiceHelper;
        private readonly IMemberService _memberService;
        private readonly ICacheableIntranetMemberService _cacheableIntranetMemberService;
        private readonly IUintraInformationService uintraInformationService;
        private readonly IMigrationHistoryService _migrationHistoryService;

        protected override string LoginViewPath => "~/Views/Login/Login.cshtml";

        public LoginController(
            IClientTimezoneProvider clientTimezoneProvider,
            IIntranetLocalizationService intranetLocalizationService,
            INotificationsService notificationsService,
            IMemberServiceHelper memberServiceHelper,
            IMemberService memberService,
            ICacheableIntranetMemberService cacheableIntranetMemberService,
            IApplicationSettings applicationSettings,
            IUintraInformationService uintraInformationService, 
            IMigrationHistoryService migrationHistoryService)
            : base(clientTimezoneProvider, intranetLocalizationService, applicationSettings)
        {
            _clientTimezoneProvider = clientTimezoneProvider;
            _intranetLocalizationService = intranetLocalizationService;
            _notificationsService = notificationsService;
            _memberServiceHelper = memberServiceHelper;
            _memberService = memberService;
            _cacheableIntranetMemberService = cacheableIntranetMemberService;
            this.uintraInformationService = uintraInformationService;
            _migrationHistoryService = migrationHistoryService;
        }

        [HttpPost]
        public async Task<JsonResult> IdTokenVerification(string idToken, string clientTimezoneId)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken,
                new GoogleJsonWebSignature.ValidationSettings()
                {
                    IssuedAtClockTolerance = TimeSpan.FromDays(1) // for cases when server's time different from UTC time (google time).
                });
            if (payload != null)
            {
                var member = _memberService.GetByEmail(payload.Email);
                if (member != null)
                {
                    FormsAuthentication.SetAuthCookie(member.Username, true);
                    _clientTimezoneProvider.SetClientTimezone(clientTimezoneId);

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
            if (Members.GetCurrentLoginStatus().IsLoggedIn)
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

        public void ApplyPermissionMigration()
        {
            var migrationVersion = new Version("1.3");
            var stepIdentity = typeof(SetupDefaultMemberGroupsPermissionsStep).Name;

            if(!_migrationHistoryService.Exists(stepIdentity, migrationVersion))
            {
                var step = DependencyResolver.Current.GetService<SetupDefaultMemberGroupsPermissionsStep>();
                var migrationItem = new MigrationItem(migrationVersion, step);
                var (executionHistory, executionResult) = MigrationHandler.TryExecuteSteps(migrationItem.AsEnumerableOfOne());
                if (executionResult.Type is ExecutionResultType.Success)
                {
                    _migrationHistoryService.Create(MigrationHandler.ToMigrationHistory(executionHistory));
                }
            }
        }

        [HttpPost]
        public ActionResult LoginUintra(LoginModel model)
        {
            model.GoogleSettings = GetGoogleSettings();
            model.CurrentIntranetVersion = uintraInformationService.Version;
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
                _clientTimezoneProvider.SetClientTimezone(model.ClientTimezoneId);

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
                _cacheableIntranetMemberService.UpdateMemberCache(mbr.Key);
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