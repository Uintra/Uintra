using System;
using System.Web.Mvc;
using System.Web.Security;
using Compent.Extensions;
using Compent.uIntra.Core.Login.Models;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants;
using Localization.Umbraco.Attributes;
using Uintra.Core;
using Uintra.Core.Localization;
using Uintra.Notification;
using Uintra.Notification.Base;
using Uintra.Notification.Configuration;
using Uintra.Users;
using Uintra.Users.Web;
using Umbraco.Core.Services;
using Umbraco.Web;

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

        protected override string LoginViewPath => "~/Views/Login/Login.cshtml";

        public LoginController(
            ITimezoneOffsetProvider timezoneOffsetProvider,
            IIntranetLocalizationService intranetLocalizationService,
            INotificationsService notificationsService,
            IMemberServiceHelper memberServiceHelper,
            IMemberService memberService,
            ICacheableIntranetUserService cacheableIntranetUserService)
            : base(timezoneOffsetProvider, intranetLocalizationService)
        {
            _timezoneOffsetProvider = timezoneOffsetProvider;
            _intranetLocalizationService = intranetLocalizationService;
            _notificationsService = notificationsService;
            _memberServiceHelper = memberServiceHelper;
            _memberService = memberService;
            _cacheableIntranetUserService = cacheableIntranetUserService;
        }

        [HttpPost]
        public JsonResult GoogleLogin(string firstName, string lastName, string email, string image, int clientTimezoneOffset)
        {
            var member = _memberService.GetByEmail(email);

            if (member != null)
            {
                FormsAuthentication.SetAuthCookie(member.Username, true);
                _timezoneOffsetProvider.SetTimezoneOffset(clientTimezoneOffset);

                return Json(new GoogleAuthResultModel
                {
                    Url = DefaultRedirectUrl,
                    Success = true
                });
            }

            return Json(new GoogleAuthResultModel
            {
                Url = DefaultRedirectUrl,
                Success = false
            });

        }

        [HttpPost]
        public override ActionResult Login(LoginModelBase model)
        {
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

                var member = Members.GetByUsername(model.Login);
                if (!_memberServiceHelper.IsFirstLoginPerformed(_memberService.GetByKey(member.GetKey())))
                {
                    SendWelcomeNotification(member.GetKey());
                }
                _memberServiceHelper.SetFirstLoginPerformed(_memberService.GetByKey(member.GetKey()));
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
                ReceiverIds = userId.ToEnumerable(),
                ActivityType = CommunicationTypeEnum.Member
            });
        }
    }
}