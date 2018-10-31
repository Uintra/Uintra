using Compent.Uintra.Core.Sync;
using Compent.Uintra.Core.Sync.Models;

namespace Compent.Uintra.Controllers.Api
{
    public class AuthCallbackController : Google.Apis.Auth.OAuth2.Mvc.Controllers.AuthCallbackController
    {
        private GmailSyncSettingsModel _settingsModel;
       
        protected override Google.Apis.Auth.OAuth2.Mvc.FlowMetadata FlowData
        {
            get
            {
                _settingsModel = (GmailSyncSettingsModel)TempData["SettingsModel"];
                return new AppFlowMetadata(_settingsModel.ClientId, _settingsModel.ClientSecret, _settingsModel.User);
            }
        }
    }
}