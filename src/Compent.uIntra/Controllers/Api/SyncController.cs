using System;
using System.Linq;
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
using Umbraco.Web.Mvc;

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
            var settingsModel = TempData[Constants.TempDataKey] as GoogleSyncSettingsModel;

            var auth = new AuthorizationCodeMvcApp(this, new AppFlowMetadata(settingsModel.ClientId, settingsModel.ClientSecret, settingsModel.User)).AuthorizeAsync(new CancellationToken());

            if (auth.Result.Credential != null)
            {
                SaveUsers(settingsModel?.Domain, auth.Result.Credential, out var e);
                return View(Constants.GoogleAuthWindowViewPath, e);
            }
            return new RedirectResult(auth.Result.RedirectUri);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Users([FromBody]GoogleSyncSettingsModel settingsModel)
        {
            var result = new JsonNetResult();
            if (!ModelState.IsValid)
            {
                result.Data = new GoogleAuthResponse()
                {
                    Success = false,
                    Message = "All fields are required!"
                };
                return result;
            }

            TempData[Constants.TempDataKey] = settingsModel;

            var auth = new AuthorizationCodeMvcApp(this, new AppFlowMetadata(settingsModel.ClientId, settingsModel.ClientSecret, settingsModel.User)).AuthorizeAsync(new CancellationToken());

            if (auth.Result.Credential != null)
            {
                SaveUsers(settingsModel.Domain, auth.Result.Credential, out var e);

                result.Data = new GoogleAuthResponse()
                {
                    Success = e == null,
                    Message = e?.Message
                };
            }
            else
                result.Data = new GoogleAuthResponse()
                {
                    Success = true,
                    Url = auth.Result.RedirectUri
                };
            return result;
        }

        private void SaveUsers(string domain, UserCredential userCredential,
            out Exception exception)
        {
            exception = null;
            try
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
            catch (Exception e)
            {
                exception = e;
            }
        }
    }
}