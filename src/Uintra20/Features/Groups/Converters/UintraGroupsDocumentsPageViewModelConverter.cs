using System;
using System.Web;
using UBaseline.Core.Node;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Media;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Groups.Converters
{
    public class UintraGroupsDocumentsPageViewModelConverter : INodeViewModelConverter<UintraGroupsDocumentsPageModel, UintraGroupsDocumentsPageViewModel>
    {
        private readonly IMediaHelper _mediaHelper;
        private readonly IGroupMemberService _groupMemberService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;

        public UintraGroupsDocumentsPageViewModelConverter(IMediaHelper mediaHelper, 
            IGroupMemberService groupMemberService, 
            IIntranetMemberService<IntranetMember> intranetMemberService)
        {
            _mediaHelper = mediaHelper;
            _groupMemberService = groupMemberService;
            _intranetMemberService = intranetMemberService;
        }

        public void Map(UintraGroupsDocumentsPageModel node, UintraGroupsDocumentsPageViewModel viewModel)
        {
            var settings = _mediaHelper.GetMediaFolderSettings(MediaFolderTypeEnum.GroupsContent);

            viewModel.AllowedMediaExtensions = settings?.AllowedMediaExtensions;

            var idStr = HttpContext.Current.Request.GetRequestQueryValue("groupId");

            if (!Guid.TryParse(idStr, out var id))
                return;
            
            viewModel.CanUpload = _groupMemberService.IsGroupMember(id, _intranetMemberService.GetCurrentMemberId());
            viewModel.GroupId = id;
        }
    }
}