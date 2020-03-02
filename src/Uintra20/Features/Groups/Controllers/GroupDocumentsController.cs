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
            var documentTasks = medias.Select(async s =>
            {
                IntranetMember creator;
                if (TryParseIntranetCreatorId(s, out var intranetCreatorId))
                {
                    creator = await _intranetMemberService.GetAsync(intranetCreatorId);
                }
                else
                {
                    creator = await _intranetMemberService.GetByUserIdAsync(s.CreatorId);
                }

                var document = groupDocumentsList.First(f => f.MediaId == s.Id);
                var model = new GroupDocumentViewModel
                {
                    CanDelete = CanDelete(currentMemberId, group, groupMembers, s),
                    Id = document.Id,
                    CreateDate = s.CreateDate.ToString("dd.MM.yyyy"),
                    Name = s.Name,
                    Type = s is FileModel file ? file.UmbracoExtension : ((ImageModel)s).UmbracoExtension,
                    Creator = creator.ToViewModel(),
                    FileUrl = s.Url
                };
                return model;
            }).ToArray();

            var documents = await Task.WhenAll(documentTasks);

            var docs = documents.OrderBy(x => x.Name);

            return docs;
        }

        private static bool TryParseIntranetCreatorId(IMediaModel s, out Guid intranetCreatorId)
        {
            intranetCreatorId = Guid.Empty;
            return !string.IsNullOrWhiteSpace(((IGenericPropertiesComposition)s).GenericProperties.IntranetUserId) && Guid.TryParse(((IGenericPropertiesComposition)s).GenericProperties.IntranetUserId, out intranetCreatorId);
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
            var canDelete = CanDelete(currentUserId, group, groupMembers, media);
            if (canDelete)
            {
                _mediaService.Remove(media.Id);
                await _groupDocumentsService.DeleteAsync(document);
                return Ok();
            }

            return BadRequest();
        }

        private bool CanDelete(Guid currentMemberId, GroupModel groupModel, IEnumerable<GroupMember> groupMembers, IMediaModel media)
        {
            if (TryParseIntranetCreatorId(media, out var mediaCreatorId))
            {
                var isMemberAdmin = _groupMemberService.IsMemberAdminOfGroup(currentMemberId, groupModel.Id);

                return currentMemberId == groupModel.CreatorId || 
                       isMemberAdmin || 
                       mediaCreatorId == currentMemberId &&
                       groupMembers.Any(s => s.MemberId == currentMemberId);
            }

            return false;
        }
    }
}