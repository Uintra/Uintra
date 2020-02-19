using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using UBaseline.Core.Controllers;
using UBaseline.Core.Media;
using UBaseline.Shared.Media;
using Uintra20.Attributes;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Core.UbaselineModels;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Groups.Sql;
using Uintra20.Infrastructure.Extensions;
using FileModel = Uintra20.Core.UbaselineModels.FileModel;
using ImageModel = Uintra20.Core.UbaselineModels.ImageModel;

namespace Uintra20.Features.Groups.Controllers
{
    [ValidateModel]
    public class GroupDocumentsController : UBaselineApiController
    {
        private readonly IGroupDocumentsService _groupDocumentsService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IGroupMemberService _groupMemberService;
        private readonly IGroupService _groupService;
        private readonly IGroupMediaService _groupMediaService;
        private readonly IMediaModelService _mediaService;

        public GroupDocumentsController(
            IGroupDocumentsService groupDocumentsService,
            IMediaModelService mediaService,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IGroupMemberService groupMemberService,
            IGroupService groupService,
            IGroupMediaService groupMediaService)
        {
            _groupDocumentsService = groupDocumentsService;
            _mediaService = mediaService;
            _intranetMemberService = intranetMemberService;
            _groupMemberService = groupMemberService;
            _groupService = groupService;
            _groupMediaService = groupMediaService;
        }

        [HttpGet]
        public async Task<IEnumerable<GroupDocumentViewModel>> List(Guid groupId)
        {
            var groupDocumentsList = await _groupDocumentsService.GetByGroupAsync(groupId);
            var mediaIdsList = groupDocumentsList.Select(s => s.MediaId).ToList();
            var medias = _mediaService.GetByIds(mediaIdsList);
            var group = await _groupService.GetAsync(groupId);
            var groupMembers = await _groupMemberService.GetGroupMemberByGroupAsync(groupId);
            var currentMemberId = await _intranetMemberService.GetCurrentMemberIdAsync();
            var docsTask = await medias.SelectAsync(async s =>
            {
                Guid? intranetCreator = null;

                if (!string.IsNullOrWhiteSpace(((IGenericPropertiesComposition) s).GenericProperties.IntranetUserId)
                && Guid.TryParse(((IGenericPropertiesComposition)s).GenericProperties.IntranetUserId, out Guid result))
                {
                    intranetCreator = result;
                }

                var creator = intranetCreator.HasValue ? _intranetMemberService.GetAsync(intranetCreator.Value) : _intranetMemberService.GetByUserIdAsync(s.CreatorId);
                var document = groupDocumentsList.First(f => f.MediaId == s.Id);
                var model = new GroupDocumentViewModel
                {
                    CanDelete = await CanDeleteAsync(currentMemberId, group, groupMembers, s),
                    Id = document.Id,
                    CreateDate = s.CreateDate.ToString("dd.MM.yyyy"),
                    Name = s.Name,
                    Type = s is FileModel file ? file.UmbracoExtension : ((ImageModel)s).UmbracoExtension,
                    Creator = (await creator).Map<MemberViewModel>(),
                    FileUrl = s.Url
                };
                return model;
            });

            var docs = docsTask.OrderBy(x => x.Name);

            return docs;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Upload(GroupDocumentCreateModel model)
        {
            var creatorId = await _intranetMemberService.GetCurrentMemberIdAsync();

            IEnumerable<int> createdMediasIds = await _groupMediaService.CreateGroupMediaAsync(model, model.GroupId, creatorId);

            await _groupDocumentsService.CreateAsync(createdMediasIds.Select(i =>
            {
                var groupDocument = new GroupDocument
                {
                    GroupId = model.GroupId,
                    MediaId = i
                };
                return groupDocument;
            }));

            return Ok();
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete(Guid groupId, Guid documentId)
        {
            var document = await _groupDocumentsService.GetAsync(documentId);
            if (document.GroupId != groupId)
            {
                throw new Exception("Can't delete document because it does not belong to this group!");
            }

            var currentUserId = await _intranetMemberService.GetCurrentMemberIdAsync();
            var group = await _groupService.GetAsync(groupId);
            var groupMembers = await _groupMemberService.GetGroupMemberByGroupAsync(groupId);
            var media = _mediaService.Get(document.MediaId);
            var canDelete = await CanDeleteAsync(currentUserId, group, groupMembers, media);
            if (canDelete)
            {
                _mediaService.Remove(media.Id);
                await _groupDocumentsService.DeleteAsync(document);
                return Ok();
            }

            return BadRequest();
        }

        private async Task<bool> CanDeleteAsync(
            Guid currentMemberId,
            GroupModel groupModel,
            IEnumerable<GroupMember> groupMembers,
            IMediaModel media)
        {
            Guid? mediaCreator = null;

            var mediaGenericProperties = (IGenericPropertiesComposition)media;

            if (!string.IsNullOrWhiteSpace(mediaGenericProperties.GenericProperties.IntranetUserId.Value)
            && Guid.TryParse(mediaGenericProperties.GenericProperties.IntranetUserId.Value, out Guid result))
            {
                mediaCreator = result;
            }

            var isMemberAdmin = await _groupMemberService.IsMemberAdminOfGroupAsync(currentMemberId, groupModel.Id);


            return currentMemberId == groupModel.CreatorId || isMemberAdmin ||
                   mediaCreator.HasValue && mediaCreator.Value == currentMemberId &&
                   groupMembers.Any(s => s.MemberId == currentMemberId);
        }
    }
}