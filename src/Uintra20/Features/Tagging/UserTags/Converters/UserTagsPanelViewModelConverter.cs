using UBaseline.Core.Node;
using Uintra20.Core.Localization;
using Uintra20.Features.Tagging.UserTags.Models;

namespace Uintra20.Features.Tagging.UserTags.Converters
{
    public class UserTagsPanelViewModelConverter : INodeViewModelConverter<UserTagsPanelModel, UserTagsPanelViewModel>
    {
        private readonly IIntranetLocalizationService _intranetLocalizationService;

        public UserTagsPanelViewModelConverter(
            IIntranetLocalizationService intranetLocalizationService)
        {
            _intranetLocalizationService = intranetLocalizationService;
        }

        public void Map(UserTagsPanelModel node, UserTagsPanelViewModel viewModel)
        {
            viewModel.Title = _intranetLocalizationService.Translate("UserTagsView.Title.lbl");

        }
    }
}