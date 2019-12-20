using Compent.Extensions;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using UBaseline.Core.Controllers;
using Uintra20.Attributes;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Bulletins.Entities;
using Uintra20.Features.Bulletins.Models;
using Uintra20.Features.Media;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Bulletins.Web
{
    [ValidateModel]
    public abstract class BulletinsControllerBase : UBaselineApiController
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
        public virtual async Task<BulletinCreationResultModel> Create(BulletinCreateModel model)
        {
            var result = new BulletinCreationResultModel();
            
            var bulletin = await MapToBulletinAsync(model);
            var createdBulletinId = await _bulletinsService.CreateAsync(bulletin);
            bulletin.Id = createdBulletinId;
            await OnBulletinCreatedAsync(bulletin, model);

            result.Id = createdBulletinId;
            result.IsSuccess = true;

            return result;
        }

        [HttpPut]
        public virtual async Task<HttpResponseMessage> Edit(BulletinEditModel editModel)
        {
            var bulletin = await MapToBulletinAsync(editModel);
            await _bulletinsService.SaveAsync(bulletin);
            await OnBulletinEditedAsync(bulletin, editModel);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpDelete]
        public virtual async Task<HttpResponseMessage> Delete(Guid id)
        {
            await _bulletinsService.DeleteAsync(id);
            await OnBulletinDeletedAsync(id);

            return new HttpResponseMessage(HttpStatusCode.OK);
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

        protected virtual async Task<BulletinBase> MapToBulletinAsync(BulletinCreateModel model)
        {
            var bulletin = model.Map<BulletinBase>();
            bulletin.PublishDate = DateTime.UtcNow;
            bulletin.CreatorId = bulletin.OwnerId = await _memberService.GetCurrentMemberIdAsync();

            if (model.NewMedia.HasValue())
            {
                bulletin.MediaIds = await _mediaHelper.CreateMediaAsync(model);
            }

            return bulletin;
        }

        protected virtual BulletinBase MapToBulletin(BulletinEditModel editModel)
        {
            var bulletin = _bulletinsService.Get(editModel.Id);
            //bulletin = editModel.Map(bulletin);
            bulletin.MediaIds = bulletin.MediaIds.Concat(_mediaHelper.CreateMedia(editModel));

            return bulletin;
        }

        protected virtual async Task<BulletinBase> MapToBulletinAsync(BulletinEditModel editModel)
        {
            var bulletin = _bulletinsService.Get(editModel.Id);
            //bulletin = editModel.Map(bulletin);
            bulletin.MediaIds = bulletin.MediaIds.Concat(await _mediaHelper.CreateMediaAsync(editModel));

            return bulletin;
        }

        protected abstract void OnBulletinCreated(BulletinBase bulletin, BulletinCreateModel model);

        protected abstract void OnBulletinEdited(BulletinBase bulletin, BulletinEditModel model);

        protected abstract void OnBulletinDeleted(Guid id);

        protected abstract Task OnBulletinCreatedAsync(BulletinBase bulletin, BulletinCreateModel model);
                           
        protected abstract Task OnBulletinEditedAsync(BulletinBase bulletin, BulletinEditModel model);
                           
        protected abstract Task OnBulletinDeletedAsync(Guid id);
    }
}