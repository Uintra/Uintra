using Compent.Uintra.Core.Sync;
using Compent.Uintra.Core.Sync.Models;

namespace Compent.Uintra.Controllers.Api
{
    [System.Web.Mvc.AllowAnonymous]
    public class AuthCallbackController : Google.Apis.Auth.OAuth2.Mvc.Controllers.AuthCallbackController
    {
        private GoogleSyncSettingsModel _settingsModel;
       
        protected override Google.Apis.Auth.OAuth2.Mvc.FlowMetadata FlowData
        {
            get
            {
                _settingsModel = (GoogleSyncSettingsModel)TempData["SettingsModel"];
                return new AppFlowMetadata(_settingsModel.ClientId, _settingsModel.ClientSecret, _settingsModel.User);
            }
        }
    }
}