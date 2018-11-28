using Compent.Uintra.Core.Sync;
using Compent.Uintra.Core.Sync.Models;
using System;
using System.Web.Mvc;

namespace Compent.Uintra.Controllers.Api
{
    [AllowAnonymous, AuthCallbackExceptionFilter]
    public class AuthCallbackController : Google.Apis.Auth.OAuth2.Mvc.Controllers.AuthCallbackController
    {
        private GoogleSyncSettingsModel _settingsModel;

        protected override Google.Apis.Auth.OAuth2.Mvc.FlowMetadata FlowData
        {
            get
            {
                _settingsModel = (GoogleSyncSettingsModel)TempData[Constants.TempDataKey];
                return new AppFlowMetadata(_settingsModel.ClientId, _settingsModel.ClientSecret, _settingsModel.User);
            }
        }

        public ActionResult Error(Exception e)
        {
            return View(Constants.GoogleAuthWindowViewPath, e);
        }
    }

    public class AuthCallbackExceptionFilterAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            var controller = (AuthCallbackController)filterContext.Controller;
            filterContext.Result = controller.Error(filterContext.Exception);
        }
    }
}