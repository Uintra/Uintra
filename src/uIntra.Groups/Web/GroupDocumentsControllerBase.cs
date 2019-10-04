using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Uintra.Core.Constants;
using Uintra.Core.Extensions;
using Uintra.Core.User;
using Uintra.Groups.Attributes;
using Uintra.Groups.Sql;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace Uintra.Groups.Web
{
    [DisabledGroupActionFilter]
    public abstract class GroupDocumentsControllerBase : SurfaceController
    {
        protected virtual string DocumentsViewPath => "~/App_Plugins/Groups/Room/Documents/Documents.cshtml";
        protected virtual string GroupDocumentsTableViewPath => "~/App_Plugins/Groups/Room/Documents/Table.cshtml";

        private readonly IGroupDocumentsService _groupDocumentsService;
        private readonly IMediaService _mediaService;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly IGroupMemberService _groupMemberService;
        private readonly IGroupService _groupService;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IGroupMediaService _groupMediaService;

        protected GroupDocumentsControllerBase(
            IGroupDocumentsService groupDocumentsService,
            IMediaService mediaService,
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            IGroupMemberService groupMemberService,
            IGroupService groupService,
            UmbracoHelper umbracoHelper,
            IGroupMediaService groupMediaService)
        {
            _groupDocumentsService = groupDocumentsService;
            _mediaService = mediaService;
            _intranetMemberService = intranetMemberService;
            _groupMemberService = groupMemberService;
            _groupService = groupService;
            _umbracoHelper = umbracoHelper;
            _groupMediaService = groupMediaService;
        }

        [NotFoundGroup]
        public virtual ActionResult Documents(Guid groupId)
        {
            var groupMembers = _groupMemberService.GetGroupMemberByGroup(groupId);
            var currentMemberId = _intranetMemberService.GetCurrentMemberId();
            var model = new GroupDocumentsViewModel
            {
                GroupId = groupId,
                CanUploadFiles = groupMembers.Any(s => s.MemberId == currentMemberId)
            };
            return PartialView(DocumentsViewPath, model);
        }

        [HttpPost]
        public ActionResult Upload(GroupDocumentCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                RedirectToCurrentUmbracoPage(Request.QueryString);
            }

            var creatorId = _intranetMemberService.GetCurrentMemberId();
            IEnumerable<int> createdMediasIds = _groupMediaService.CreateGroupMedia(model, model.GroupId, creatorId);

            _groupDocumentsService.Create(createdMediasIds.Select(i =>
                {
                    var groupDocument = new GroupDocument
                        {
                            GroupId = model.GroupId,
                            MediaId = i
                        };
                    return groupDocument;
                }));

            return RedirectToCurrentUmbracoPage(Request.QueryString);
        }

        public virtual ActionResult DocumentsTable(Guid groupId, GroupDocumentDocumentField column, Direction direction)
        {
            var groupDocumentsList = _groupDocumentsService.GetByGroup(groupId).ToList();
            var mediaIdsList = groupDocumentsList.Select(s => s.MediaId).ToList();
            var medias = _mediaService.GetByIds(mediaIdsList);
            var group = _groupService.Get(groupId);
            var groupMembers = _groupMemberService.GetGroupMemberByGroup(groupId);
            var currentMember = _intranetMemberService.GetCurrentMember();
            var docs = medias.Select(s =>
            {
                var intranetCreator = s.GetValue<Guid?>(IntranetConstants.IntranetCreatorId);
                var creator = intranetCreator.HasValue ? _intranetMemberService.Get(intranetCreator.Value) : _intranetMemberService.GetByUserId(s.CreatorId);
                var document = groupDocumentsList.First(f => f.MediaId == s.Id);
                var url = _umbracoHelper.TypedMedia(s.Id).Url;
                var model = new GroupDocumentTableRowViewModel
                {
                    CanDelete = CanDelete(currentMember, group, groupMembers, s),
                    Id = document.Id,
                    CreateDate = s.CreateDate,
                    Name = s.Name,
                    Type = s.GetValue<string>(UmbracoAliases.Media.MediaExtension),
                    Creator = creator.Map<MemberViewModel>(),
                    FileUrl = url
                };
                return model;
            });

            docs = Sort(docs, column, direction);

            var viewModel = new GroupDocumentsTableViewModel
            {
                Documents = docs,
                GroupId = groupId,
                Column = column,
                Direction = direction
            };
            return PartialView(GroupDocumentsTableViewPath, viewModel);
        }

        [HttpPost]
        public virtual ActionResult DeleteDocument(Guid groupId, Guid documentId, GroupDocumentDocumentField column, Direction direction)
        {
            var document = _groupDocumentsService.Get(documentId);
            if (document.GroupId != groupId)
            {
                throw new Exception("Can't delete document because it does not belong to this group!");
            }

            var currentUser = _intranetMemberService.GetCurrentMember();
            var group = _groupService.Get(groupId);
            var groupMembers = _groupMemberService.GetGroupMemberByGroup(groupId);
            var media = _mediaService.GetById(document.MediaId);
            var canDelete = CanDelete(currentUser, group, groupMembers, media);
            if (canDelete)
            {
                _mediaService.Delete(media);
                _groupDocumentsService.Delete(document);
            }

            return DocumentsTable(groupId, column, direction);
        }

        protected virtual bool CanDelete(
            IIntranetMember currentMember, 
            GroupModel groupModel, 
            IEnumerable<GroupMember> groupMembers, 
            IMedia media)
        {
            var mediaCreator = media.GetValue<Guid?>(IntranetConstants.IntranetCreatorId);

            var isMemberAdmin = _groupMemberService.IsMemberAdminOfGroup(currentMember.Id, groupModel.Id);

            return currentMember.Id == groupModel.CreatorId || isMemberAdmin ||
                mediaCreator.HasValue && mediaCreator.Value == currentMember.Id &&
                groupMembers.Any(s => s.MemberId == currentMember.Id);
        }

        protected IEnumerable<GroupDocumentTableRowViewModel> Sort(IEnumerable<GroupDocumentTableRowViewModel> documents, GroupDocumentDocumentField column, Direction direction)
        {
            var keySelector = GetKeySelector(column);

            switch (direction)
            {
                case Direction.Asc:
                    documents = documents.OrderBy(keySelector);
                    break;
                case Direction.Desc:
                    documents = documents.OrderByDescending(keySelector);
                    break;
            }

            return documents;
        }

        protected Func<GroupDocumentTableRowViewModel, object> GetKeySelector(GroupDocumentDocumentField column)
        {
            Func<GroupDocumentTableRowViewModel, object> keySelector;

            switch (column)
            {
                case GroupDocumentDocumentField.Type:
                    keySelector = s => s.Type;
                    break;
                case GroupDocumentDocumentField.Date:
                    keySelector = s => s.CreateDate;
                    break;
                case GroupDocumentDocumentField.Creator:
                    keySelector = s => s.Creator.DisplayedName;
                    break;
                default:
                    keySelector = s => s.Name;
                    break;
            }
            return keySelector;
        }
    }
}