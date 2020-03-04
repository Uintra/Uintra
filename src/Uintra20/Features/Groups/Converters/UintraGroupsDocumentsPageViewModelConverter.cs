using System;
using System.Web;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Core.UbaselineModels.RestrictedNode;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Links;
using Uintra20.Features.Media;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Groups.Converters
{
    public class UintraGroupsDocumentsPageViewModelConverter : UintraRestrictedNodeViewModelConverter<UintraGroupsDocumentsPageModel, UintraGroupsDocumentsPageViewModel>
    {
        private readonly IMediaHelper _mediaHelper;
        private readonly IGroupMemberService _groupMemberService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;

        public UintraGroupsDocumentsPageViewModelConverter(IMediaHelper mediaHelper, 
            IGroupMemberService groupMemberService, 
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IErrorLinksService errorLinksService)
        : base(errorLinksService)
        {
            _mediaHelper = mediaHelper;
            _groupMemberService = groupMemberService;
            _intranetMemberService = intranetMemberService;
        }

        public override ConverterResponseModel MapViewModel(UintraGroupsDocumentsPageModel node, UintraGroupsDocumentsPageViewModel viewModel)
        {
            var settings = _mediaHelper.GetMediaFolderSettings(MediaFolderTypeEnum.GroupsContent);

            viewModel.AllowedMediaExtensions = settings?.AllowedMediaExtensions;

            var idStr = HttpContext.Current.Request.GetRequestQueryValue("groupId");

            if (!Guid.TryParse(idStr, out var id))
                return NotFoundResult();
            
            viewModel.CanUpload = _groupMemberService.IsGroupMember(id, _intranetMemberService.GetCurrentMemberId());
            viewModel.GroupId = id;

            return OkResult();
        }
    }
}