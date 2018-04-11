using System;
using System.Web.Mvc;
using System.Web.Security;
using Extensions;
using Localization.Umbraco.Attributes;
using uIntra.Notification;
using Uintra.Core;
using Uintra.Core.Localization;
using Uintra.Notification;
using Uintra.Notification.Base;
using Uintra.Notification.Configuration;
using Uintra.Users;
using Uintra.Users.Web;
using Umbraco.Core;
using Umbraco.Core.Models;
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

        protected override string LoginViewPath => "~/Views/Login/Login.cshtml";

        public LoginController(
            ITimezoneOffsetProvider timezoneOffsetProvider,
            IIntranetLocalizationService intranetLocalizationService,
            INotificationsService notificationsService) :
            base(timezoneOffsetProvider, intranetLocalizationService)
        {
            _timezoneOffsetProvider = timezoneOffsetProvider;
            _intranetLocalizationService = intranetLocalizationService;
            _notificationsService = notificationsService;
        }

        [HttpPost]
        public override ActionResult Login(LoginModelBase model)
        {
            if (!ModelState.IsValid)
            {
                return View(LoginViewPath, model);
            }

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
                if (IsFirstTimeLogin(member))
                {
                    SendWelcomeNotification(member.GetKey());
                }
            }

            return Redirect(redirectUrl);
        }

        private static bool IsFirstTimeLogin(IPublishedContent member)
        {
            var createDate = member.CreateDate;
            var lastLoginDate = member.GetPropertyValue<DateTime>(Constants.Conventions.Member.LastLoginDate);
            var isMemberNeverLogin = (lastLoginDate - createDate).TotalMinutes < 1;
            return isMemberNeverLogin;
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