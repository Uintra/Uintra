using System;
using System.Collections.Generic;
using System.Web.Http;
using uIntra.Core.Extentions;
using uIntra.Core.User;
using Umbraco.Web.WebApi;

namespace uIntra.Bulletins
{
    public class BulletinsSectionController : UmbracoAuthorizedApiController
    {
        private readonly IBulletinsService<BulletinBase> _bulletinsService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        public BulletinsSectionController(IBulletinsService<BulletinBase> bulletinsService, IIntranetUserService<IIntranetUser> intranetUserService)
        {
            _bulletinsService = bulletinsService;
            _intranetUserService = intranetUserService;
        }

        public IEnumerable<BulletinsBackofficeViewModel> GetAll()
        {
            var bulletins = _bulletinsService.GetAll(true);
            foreach (var bulletin in bulletins)
            {
                bulletin.CreatorId = _intranetUserService.GetCreator(bulletin).Id;
            }

            var result = bulletins.Map<IEnumerable<BulletinsBackofficeViewModel>>();
            return result;
        }

        [HttpPost]
        public BulletinsBackofficeViewModel Create(BulletinsBackofficeCreateModel createModel)
        {
            var bulletinId = _bulletinsService.Create(createModel.Map<BulletinBase>());
            var createdModel = _bulletinsService.Get(bulletinId);

            var result = createdModel.Map<BulletinsBackofficeViewModel>();
            result.CreatorId = _intranetUserService.GetCreator(createdModel).Id;
            return result;
        }

        [HttpPost]
        public BulletinsBackofficeViewModel Save(BulletinsBackofficeSaveModel saveModel)
        {
            _bulletinsService.Save(saveModel.Map<BulletinBase>());

            var updatedModel = _bulletinsService.Get(saveModel.Id);
            var result = updatedModel.Map<BulletinsBackofficeViewModel>();
            result.CreatorId = _intranetUserService.GetCreator(updatedModel).Id;
            return result;
        }

        [HttpDelete]
        public void Delete(Guid id)
        {
            _bulletinsService.Delete(id);
        }
    }
}