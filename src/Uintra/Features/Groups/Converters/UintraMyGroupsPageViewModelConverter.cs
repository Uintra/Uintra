using UBaseline.Core.Node;
using Uintra.Features.Groups.Helpers;
using Uintra.Features.Groups.Models;

namespace Uintra.Features.Groups.Converters
{
    public class UintraMyGroupsPageViewModelConverter : INodeViewModelConverter<UintraMyGroupsPageModel, UintraMyGroupsPageViewModel>
    {

        private readonly IGroupHelper _groupHelper;

        public UintraMyGroupsPageViewModelConverter(IGroupHelper groupHelper)
        {
            _groupHelper = groupHelper;
        }

        public void Map(UintraMyGroupsPageModel node, UintraMyGroupsPageViewModel viewModel)
        {
            viewModel.Navigation = _groupHelper.GroupNavigation();
        }
    }
}