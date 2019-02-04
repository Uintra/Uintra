using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using Uintra.Core.Extensions;
using Uintra.Core.User;
using Umbraco.Web.WebApi;

namespace Uintra.Bulletins
{
    public abstract class BulletinsSectionControllerBase : UmbracoAuthorizedApiController
    {
        private readonly IBulletinsService<BulletinBase> _bulletinsService;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;

        protected BulletinsSectionControllerBase(IBulletinsService<BulletinBase> bulletinsService, IIntranetMemberService<IIntranetMember> intranetMemberService)
        {
            _bulletinsService = bulletinsService;
            _intranetMemberService = intranetMemberService;
        }

        public virtual IEnumerable<BulletinsBackofficeViewModel> GetAll()
        {
            var bulletins = _bulletinsService.GetAll(true);
            var result = bulletins.Map<IEnumerable<BulletinsBackofficeViewModel>>();
            return result;
        }

        [HttpPost]
        public virtual BulletinsBackofficeViewModel Create(BulletinsBackofficeCreateModel createModel)
        {
            var creatingBulletin = createModel.Map<BulletinBase>();
            creatingBulletin.CreatorId = creatingBulletin.OwnerId = _intranetMemberService.GetCurrentMemberId();

            var bulletinId = _bulletinsService.Create(creatingBulletin);
            var createdBulletin = _bulletinsService.Get(bulletinId);

            var result = createdBulletin.Map<BulletinsBackofficeViewModel>();
            return result;
        }

        [HttpPost]
        public virtual BulletinsBackofficeViewModel Save(BulletinsBackofficeSaveModel saveModel)
        {
            var bulletin = _bulletinsService.Get(saveModel.Id);
            bulletin = Mapper.Map(saveModel, bulletin);
            _bulletinsService.Save(bulletin);

            var updatedBulletin = _bulletinsService.Get(saveModel.Id);
            var result = updatedBulletin.Map<BulletinsBackofficeViewModel>();
            return result;
        }

        [HttpDelete]
        public virtual void Delete(Guid id)
        {
            _bulletinsService.Delete(id);
        }
    }
}