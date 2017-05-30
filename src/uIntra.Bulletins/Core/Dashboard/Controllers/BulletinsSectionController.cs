using System;
using System.Collections.Generic;
using System.Web.Http;
using uIntra.Core.Extentions;
using Umbraco.Web.WebApi;

namespace uIntra.Bulletins
{
    public class BulletinsSectionController : UmbracoAuthorizedApiController
    {
        private readonly IBulletinsService<BulletinBase> _bulletinsService;

        public BulletinsSectionController(IBulletinsService<BulletinBase> bulletinsService)
        {
            _bulletinsService = bulletinsService;
        }

        public IEnumerable<BulletinsBackofficeViewModel> GetAll()
        {
            var bulletins = _bulletinsService.GetAll(includeHidden: true);
            var result = bulletins.Map<IEnumerable<BulletinsBackofficeViewModel>>();
            return result;
        }

        [HttpPost]
        public BulletinsBackofficeViewModel Create(BulletinsBackofficeCreateModel createModel)
        {
            var bulletinId = _bulletinsService.Create(createModel.Map<BulletinBase>());
            var createdModel = _bulletinsService.Get(bulletinId);

            var result = createdModel.Map<BulletinsBackofficeViewModel>();
            return result;
        }

        [HttpPost]
        public BulletinsBackofficeViewModel Save(BulletinsBackofficeSaveModel saveModel)
        {
            _bulletinsService.Save(saveModel.Map<BulletinBase>());

            var updatedModel = _bulletinsService.Get(saveModel.Id);
            var result = updatedModel.Map<BulletinsBackofficeViewModel>();
            return result;
        }

        [HttpDelete]
        public void Delete(Guid id)
        {
            _bulletinsService.Delete(id);
        }
    }
}