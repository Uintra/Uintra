using System;
using System.Web;
using UBaseline.Core.Node;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Media;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Groups.Converters
{
    public class UintraGroupsDocumentsPageViewModelConverter : INodeViewModelConverter<UintraGroupsDocumentsPageModel, UintraGroupsDocumentsPageViewModel>
    {
        private readonly IMediaHelper _mediaHelper;

        public UintraGroupsDocumentsPageViewModelConverter(IMediaHelper mediaHelper)
        {
            _mediaHelper = mediaHelper;
        }

        public void Map(UintraGroupsDocumentsPageModel node, UintraGroupsDocumentsPageViewModel viewModel)
        {
            var settings = _mediaHelper.GetMediaFolderSettings(MediaFolderTypeEnum.GroupsContent);

            viewModel.AllowedMediaExtensions = settings?.AllowedMediaExtensions;

            var idStr = HttpContext.Current.Request.GetRequestQueryValue("groupId");

            if (!Guid.TryParse(idStr, out var id))
                return;
            
            viewModel.GroupId = id;
        }
    }
}