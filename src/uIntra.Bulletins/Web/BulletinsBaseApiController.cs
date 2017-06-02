using System;
using System.Linq;
using System.Web.Http;
using uIntra.Core;
using uIntra.Core.Extentions;
using uIntra.Core.Media;
using uIntra.Core.User;
using Umbraco.Web.WebApi;

namespace uIntra.Bulletins.Web
{
    public abstract class BulletinsBaseApiController : UmbracoApiController
    {
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IBulletinsService<BulletinBase> _bulletinService;
        private readonly IMediaHelper _mediaHelper;

        protected BulletinsBaseApiController(
            IIntranetUserService<IIntranetUser> intranetUserService,
            IBulletinsService<BulletinBase> bulletinService,
            IMediaHelper mediaHelper)
        {
            _intranetUserService = intranetUserService;
            _bulletinService = bulletinService;
            _mediaHelper = mediaHelper;
        }

        [HttpPost]
        public virtual BulletinCreationResultModel Create(BulletinCreateModel model)
        {
            var result = new BulletinCreationResultModel();

            if (!ModelState.IsValid)
            {
                return result;
            }

            var bulletin = model.Map<BulletinBase>();
            bulletin.PublishDate = DateTime.UtcNow;
            bulletin.CreatorId = _intranetUserService.GetCurrentUserId();

            if (model.NewMedia.IsNotNullOrEmpty())
            {
                var mediaSettings = _bulletinService.GetMediaSettings();
                model.MediaRootId = mediaSettings.MediaRootId;

                bulletin.MediaIds = bulletin.MediaIds.Concat(_mediaHelper.CreateMedia(model));
            }

            var activityId = _bulletinService.Create(bulletin);
            OnEventCreated(activityId);

            result.IsSuccess = true;
            return result;
        }

        protected virtual void OnEventCreated(Guid activityId)
        {
        }
    }
}
