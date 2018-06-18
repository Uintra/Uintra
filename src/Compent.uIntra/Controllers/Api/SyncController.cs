using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Http;
using System.Web.Mvc;
using Compent.Uintra.Core.Sync;
using Compent.Uintra.Core.Sync.Models;
using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Services;
using Uintra.Core.Extensions;
using Uintra.Core.User;
using Uintra.Core.User.DTO;

namespace Compent.Uintra.Controllers.Api
{
    [System.Web.Mvc.AllowAnonymous]
    public class SyncController : Controller
    {
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        public SyncController(IIntranetUserService<IIntranetUser> intranetUserService)
        {
            _intranetUserService = intranetUserService;
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Users()
        {
            var settingsModel = TempData["SettingsModel"] as GmailSyncSettingsModel;

            var result = new AuthorizationCodeMvcApp(this, new AppFlowMetadata(settingsModel?.ClientId, settingsModel?.ClientSecret, settingsModel?.User)).AuthorizeAsync(new CancellationToken());

            if (result.Result.Credential != null)
            {
                SaveUsers(settingsModel?.Domain, result.Result.Credential);

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            return new RedirectResult(result.Result.RedirectUri);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Users([FromBody]GmailSyncSettingsModel settingsModel)
        {
            TempData["SettingsModel"] = settingsModel;

            var result = new AuthorizationCodeMvcApp(this, new AppFlowMetadata(settingsModel?.ClientId, settingsModel?.ClientSecret, settingsModel?.User)).AuthorizeAsync(new CancellationToken());

            if (result.Result.Credential != null)
            {
                SaveUsers(settingsModel?.Domain, result.Result.Credential);

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            return new RedirectResult(result.Result.RedirectUri);
        }

        private void SaveUsers(string domain, UserCredential userCredential)
        {
            var service = new DirectoryService(new BaseClientService.Initializer
            {
                HttpClientInitializer = userCredential,
                ApplicationName = "Uintra"
            });

            var request = service.Users.List();
            request.Domain = domain;
            request.OrderBy = UsersResource.ListRequest.OrderByEnum.Email;            
            var list = request.Execute();

            if (list.UsersValue.Any())
            {
                foreach (var user in list.UsersValue)
                {
                    var createUserDto = user.Map<CreateUserDto>();
                    _intranetUserService.Create(createUserDto); //todo: reimplement for batch create
                }
            }
        }
    }
}