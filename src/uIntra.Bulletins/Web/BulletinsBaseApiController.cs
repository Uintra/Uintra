using System;
using System.Web.Http;
using uIntra.Core;
using uIntra.Core.Extentions;
using uIntra.Core.User;
using Umbraco.Web.WebApi;

namespace uIntra.Bulletins.Web
{
    public abstract class BulletinsBaseApiController : UmbracoApiController
    {
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IBulletinsService<BulletinBase> _bulletinService;

        protected BulletinsBaseApiController(
            IIntranetUserService<IIntranetUser> intranetUserService,
            IBulletinsService<BulletinBase> bulletinService
            )
        {
            _intranetUserService = intranetUserService;
            _bulletinService = bulletinService;
        }

        [HttpPost]
        public virtual BulletinCreationResultModel Create(BulletinCreateModel model)
        {
            var result = new BulletinCreationResultModel();

            if (!ModelState.IsValid)
            {
                return result;
            }

            var bulletin = model.Map<BulletinBase>(); // TODO: automapper ?
            bulletin.PublishDate = DateTime.UtcNow;
            //bulletin.MediaIds = bulletin.MediaIds.Concat(_mediaHelper.CreateMedia(createModel));
            bulletin.CreatorId = _intranetUserService.GetCurrentUserId();

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
