using Google.Apis.Auth.OAuth2;
using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Compent.Uintra.Core.Sync.Models;
using Uintra.Core.Extensions;
using Uintra.Core.User;
using Uintra.Core.User.DTO;
using Umbraco.Core.Services;
using Umbraco.Core.Models;

namespace Compent.uIntra.Core.Sync
{
    public class UintraSyncService : ISyncService
    {
        private readonly IMemberService _memberService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        static readonly string[] Scopes = { DirectoryService.Scope.AdminDirectoryUserReadonly };
        static string ApplicationName = "uIntra";

        public UintraSyncService(
            IMemberService memberService,
            IIntranetUserService<IIntranetUser> intranetUserService)
        {
            _memberService = memberService;
            _intranetUserService = intranetUserService;
        }

        private IList<User> GetGmailUsers(GmailSyncSettingsModel model)
        {
            string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            credPath = Path.Combine(credPath, ".credentials/", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);

            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = model.ClientId,
                    ClientSecret = model.ClientSecret
                },
                Scopes,
                model.ClientSecret,
                CancellationToken.None,
                new FileDataStore(credPath, true)).Result;


            var service = new DirectoryService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            UsersResource.ListRequest request = service.Users.List();
            request.Domain = model.Domain;
            request.MaxResults = 10;
            request.OrderBy = UsersResource.ListRequest.OrderByEnum.Email;

            IList<User> users = request.Execute().UsersValue;
            return users;
        }

        public void Sync(GmailSyncSettingsModel model)
        {
            var syncSettingsModel = new GmailSyncSettingsModel()
            {
                ClientId = "67034246466-st2pm9kucp953bvk1odor42609lvd34i.apps.googleusercontent.com",
                ClientSecret = "FPtSqaEm6IUDTr6MIog3FiJg",
                Domain = "compent.net",
                User = "dpr@compenrt.net"
            };

            var users = GetGmailUsers(syncSettingsModel);

            if (users.Any())
            {
                foreach (var user in users)
                {
                    var createUserDto = user.Map<CreateUserDto>();
                    _intranetUserService.Create(createUserDto); //todo: reimplement for batch create
                }
            }
        }

        public void Sync()
        {
            throw new System.NotImplementedException();
        }
    }
}