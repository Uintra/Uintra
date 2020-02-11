using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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
using Uintra20.Infrastructure.Constants;
using Uintra20.Infrastructure.Extensions;

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

        //[HttpPost]
        //public async Task<IHttpActionResult> Upload(GroupDocumentCreateModel model)
        //{
        //    var creatorId = await _intranetMemberService.GetCurrentMemberIdAsync();
        //    IEnumerable<int> createdMediasIds = _groupMediaService.CreateGroupMedia(model, model.GroupId, creatorId);

        //    await _groupDocumentsService.CreateAsync(createdMediasIds.Select(i =>
        //    {
        //        var groupDocument = new GroupDocument
        //        {
        //            GroupId = model.GroupId,
        //            MediaId = i
        //        };
        //        return groupDocument;
        //    }));

        //    return RedirectToCurrentUmbracoPage(Request.QueryString);
        //}

        //[HttpPost]
        //public IHttpActionResult DeleteDocument(Guid groupId, Guid documentId, GroupDocumentDocumentField column, Direction direction)
        //{
        //    var document = _groupDocumentsService.Get(documentId);
        //    if (document.GroupId != groupId)
        //    {
        //        throw new Exception("Can't delete document because it does not belong to this group!");
        //    }

        //    var currentUser = _intranetMemberService.GetCurrentMember();
        //    var group = _groupService.Get(groupId);
        //    var groupMembers = _groupMemberService.GetGroupMemberByGroup(groupId);
        //    var media = _mediaService.Get(document.MediaId);
        //    var canDelete = CanDelete(currentUser, group, groupMembers, media);
        //    if (canDelete)
        //    {
        //        _mediaService.Remove(media.Id);
        //        _groupDocumentsService.Delete(document);
        //    }

        //    return DocumentsTable(groupId, column, direction);
        //}

        //private bool CanDelete(
        //    IntranetMember currentMember,
        //    GroupModel groupModel,
        //    IEnumerable<GroupMember> groupMembers,
        //    IMediaModel media)
        //{
        //    Guid? mediaCreator = null;

        //    var mediaGenericProperties = (IGenericPropertiesComposition) media;

        //    if (string.IsNullOrWhiteSpace(mediaGenericProperties.GenericProperties.IntranetUserId.Value)
        //    && Guid.TryParse(mediaGenericProperties.GenericProperties.IntranetUserId, out Guid result))
        //    {
        //        mediaCreator = result;
        //    }

        //    var isMemberAdmin = _groupMemberService.IsMemberAdminOfGroup(currentMember.Id, groupModel.Id);

            
        //    return currentMember.Id == groupModel.CreatorId || isMemberAdmin ||
        //           mediaCreator.HasValue && mediaCreator.Value == currentMember.Id &&
        //           groupMembers.Any(s => s.MemberId == currentMember.Id);
        //}
    }
}