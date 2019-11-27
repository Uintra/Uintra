using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Compent.Extensions;
using Compent.Shared.Extensions;
using Uintra20.Attributes;
using Uintra20.Core.Member;
using Uintra20.Core.Member.Entities;
using Uintra20.Features.Bulletins.Entities;
using Uintra20.Features.Bulletins.Models;
using Uintra20.Features.Media;
using Uintra20.Infrastructure.Extensions;
using Umbraco.Web.WebApi;

namespace Uintra20.Features.Bulletins.Web
{
    [ValidateModel]
    public abstract class BulletinsControllerBase : UmbracoApiController
    {
        private readonly IBulletinsService<Bulletin> _bulletinsService;
        private readonly IMediaHelper _mediaHelper;
        private readonly IIntranetMemberService<IntranetMember> _memberService;

        protected BulletinsControllerBase(
            IBulletinsService<Bulletin> bulletinsService,
            IMediaHelper mediaHelper,
            IIntranetMemberService<IntranetMember> memberService)
        {
            _bulletinsService = bulletinsService;
            _mediaHelper = mediaHelper;
            _memberService = memberService;
        }

        [HttpPost]
        public virtual BulletinCreationResultModel Create(BulletinCreateModel model)
        {
            var result = new BulletinCreationResultModel();
            
            var bulletin = MapToBulletin(model);
            var createdBulletinId = _bulletinsService.Create(bulletin);
            bulletin.Id = createdBulletinId;
            OnBulletinCreated(bulletin, model);

            result.Id = createdBulletinId;
            result.IsSuccess = true;

            return result;
        }

        [HttpPut]
        public virtual HttpResponseMessage Edit(BulletinEditModel editModel)
        {
            var bulletin = MapToBulletin(editModel);
            _bulletinsService.Save(bulletin);
            OnBulletinEdited(bulletin, editModel);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpDelete]
        public virtual object Delete(Guid id)
        {
            _bulletinsService.Delete(id);
            OnBulletinDeleted(id);

            return new { IsSuccess = true };
        }

        protected virtual BulletinBase MapToBulletin(BulletinCreateModel model)
        {
            var bulletin = model.Map<BulletinBase>();
            bulletin.PublishDate = DateTime.UtcNow;
            bulletin.CreatorId = bulletin.OwnerId = _memberService.GetCurrentMemberId();

            if (model.NewMedia.HasValue())
            {
                bulletin.MediaIds = _mediaHelper.CreateMedia(model);
            }

            return bulletin;
        }

        protected virtual BulletinBase MapToBulletin(BulletinEditModel editModel)
        {
            var bulletin = _bulletinsService.Get(editModel.Id);
            bulletin = editModel.Map(bulletin);
            bulletin.MediaIds = bulletin.MediaIds.Concat(_mediaHelper.CreateMedia(editModel));

            return bulletin;
        }

        protected abstract void OnBulletinCreated(BulletinBase bulletin, BulletinCreateModel model);

        protected abstract void OnBulletinEdited(BulletinBase bulletin, BulletinEditModel model);

        protected abstract void OnBulletinDeleted(Guid id);
    }
}