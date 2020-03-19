using System;
using System.Web;
using UBaseline.Core.Node;
using Uintra20.Features.Groups.Helpers;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Core.UbaselineModels.RestrictedNode;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Links;
using Uintra20.Features.Media;
using Uintra20.Features.Media.Enums;
using Uintra20.Features.Media.Helpers;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Groups.Converters
{
    public class UintraGroupsDocumentsPageViewModelConverter : UintraRestrictedNodeViewModelConverter<UintraGroupsDocumentsPageModel, UintraGroupsDocumentsPageViewModel>
    {
        private readonly IMediaHelper _mediaHelper;
        private readonly IGroupDocumentsService _groupDocumentsService;
        private readonly IGroupHelper _groupHelper;

        public UintraGroupsDocumentsPageViewModelConverter(
            IMediaHelper mediaHelper,
            IGroupDocumentsService groupDocumentsService,
            IGroupHelper groupHelper,
            IErrorLinksService errorLinksService)
            : base(errorLinksService)
        {
            _mediaHelper = mediaHelper;
            _groupDocumentsService = groupDocumentsService;
            _groupHelper = groupHelper;
        }

        public override ConverterResponseModel MapViewModel(UintraGroupsDocumentsPageModel node, UintraGroupsDocumentsPageViewModel viewModel)
        {
            var settings = _mediaHelper.GetMediaFolderSettings(MediaFolderTypeEnum.GroupsContent);

            viewModel.AllowedMediaExtensions = settings?.AllowedMediaExtensions;

            var idStr = HttpContext.Current.Request.GetRequestQueryValue("groupId");

            if (!Guid.TryParse(idStr, out var id))
                return NotFoundResult();
            
            viewModel.CanUpload = _groupDocumentsService.CanUpload(id);
            viewModel.GroupHeader = _groupHelper.GetHeader(id);
            viewModel.GroupId = id;

            return OkResult();
        }
    }
}