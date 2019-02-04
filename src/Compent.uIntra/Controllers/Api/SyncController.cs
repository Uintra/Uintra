using System;
using System.Linq;
using System.Threading;
using System.Web.Http;
using System.Web.Mvc;
using Compent.Uintra.Core.Sync;
using Compent.Uintra.Core.Sync.Models;
using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Services;
using Uintra.Core.Controls.FileUpload;
using Uintra.Core.Exceptions;
using Uintra.Core.Extensions;
using Uintra.Core.Media;
using Uintra.Core.User;
using Uintra.Core.User.DTO;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace Compent.Uintra.Controllers.Api
{
    [System.Web.Mvc.AllowAnonymous]
    public class SyncController : Controller
    {
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly IMediaHelper _mediaHelper;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IExceptionLogger _exceptionLogger;


        public SyncController(IIntranetMemberService<IIntranetMember> intranetMemberService,
            IMediaHelper mediaHelper, UmbracoHelper umbracoHelper, IExceptionLogger exceptionLogger)
        {
            _intranetMemberService = intranetMemberService;
            _mediaHelper = mediaHelper;
            _umbracoHelper = umbracoHelper;
            _exceptionLogger = exceptionLogger;
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Users()
        {
            var settingsModel = TempData[Constants.TempDataKey] as GoogleSyncSettingsModel;

            var auth = new AuthorizationCodeMvcApp(this, new AppFlowMetadata(settingsModel.ClientId, settingsModel.ClientSecret, settingsModel.User)).AuthorizeAsync(new CancellationToken());

            if (auth.Result.Credential != null)
            {
                SaveUsers(settingsModel?.Domain, settingsModel.UpdateExisting,
                    auth.Result.Credential, out var e);
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
                SaveUsers(settingsModel.Domain, settingsModel.UpdateExisting,
                    auth.Result.Credential, out var e);

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

        private void SaveUsers(string domain, bool updateExisting,
            UserCredential userCredential, out Exception exception)
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
                var users = request.Execute().UsersValue.ToList();

                foreach (var user in users)
                {
                    var email = user.Emails.First().Address;
                    var existingUser = _intranetMemberService.GetByEmail(email);

                    if (existingUser != null)
                    {
                        if (updateExisting)
                        {
                            var updateUserDto = user.Map<UpdateMemberDto>();
                            updateUserDto.Id = existingUser.Id;
                            updateUserDto.NewMedia = GetMediaId(user, service);
                            _intranetMemberService.Update(updateUserDto);
                        }
                    }
                    else
                    {
                        var createUserDto = user.Map<CreateMemberDto>();
                        createUserDto.MediaId = GetMediaId(user, service);
                        _intranetMemberService.Create(createUserDto);
                    }
                }
            }
            catch (Exception e)
            {
                exception = e;
            }
        }

        private int? GetMediaId(User user, DirectoryService service)
        {
            if (string.IsNullOrWhiteSpace(user.ThumbnailPhotoUrl))
                return null;
            try
            {
                var userPhoto = service.Users.Photos.Get(user.Id).Execute();
                byte[] file = Base64UrlDecode(userPhoto.PhotoData);
                var mediaSettings = _mediaHelper.GetMediaFolderSettings(MediaFolderTypeEnum.MembersContent);
                var media = _mediaHelper.CreateMedia(new TempFile()
                {
                    FileBytes = file,
                    FileName = user.Name.FullName + "." + userPhoto.MimeType.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[1]
                }, mediaSettings.MediaRootId.Value);
                return media?.Id;
            }
            catch (Google.GoogleApiException gApiException)
            {
                if (gApiException.Error.Code != 404)
                    _exceptionLogger.Log(gApiException);
            }
            catch (Exception ex)
            {
                _exceptionLogger.Log(ex);
            }
            return null;
        }

        private static byte[] Base64UrlDecode(string base64Url)
        {
            var base64 = base64Url.Replace('-', '+').Replace('_', '/');
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}