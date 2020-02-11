using UBaseline.Core.Node;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Media;

namespace Uintra20.Features.Groups.Converters
{
    public class UintraGroupsCreatePageViewModelConverter : INodeViewModelConverter<UintraGroupsCreatePageModel, UintraGroupsCreatePageViewModel>
    {
        private readonly IMediaHelper _mediaHelper;

        public UintraGroupsCreatePageViewModelConverter(IMediaHelper mediaHelper)
        {
            _mediaHelper = mediaHelper;
        }

        public void Map(UintraGroupsCreatePageModel node, UintraGroupsCreatePageViewModel viewModel)
        {
            var settings = _mediaHelper.GetMediaFolderSettings(MediaFolderTypeEnum.GroupsContent);

            viewModel.AllowedMediaExtensions = settings?.AllowedMediaExtensions;
        }
    }
}