using System.IO;
using System.Web.Hosting;
using System.Web.Mvc;
using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Util.Store;

namespace Compent.Uintra.Core.Sync
{
    public class AppFlowMetadata : FlowMetadata
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _userId;
        private readonly string _dataStorePath;

        public AppFlowMetadata(string clientId, string clientSecret, string userId)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _userId = userId;
            _dataStorePath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, @"App_Data\Sync");
        }

        public override string GetUserId(Controller controller)
        {
            return _userId;
        }

        public override IAuthorizationCodeFlow Flow => new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets
            {
                ClientId = _clientId,
                ClientSecret = _clientSecret
            },
            Scopes = new[] { DirectoryService.Scope.AdminDirectoryUserReadonly },
            DataStore = new FileDataStore(_dataStorePath)
        });
    }
}