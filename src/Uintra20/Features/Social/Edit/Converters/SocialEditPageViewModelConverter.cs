using System;
using System.Linq;
using System.Web;
using UBaseline.Core.Localization;
using UBaseline.Core.Node;
using Uintra20.Core.Controls.LightboxGallery;
using Uintra20.Features.Media.Strategies.Preset;
using Uintra20.Features.Social.Edit.Models;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Extensions;
using Umbraco.Core.Services;

namespace Uintra20.Features.Social.Edit.Converters
{
    public class SocialEditPageViewModelConverter
        : INodeViewModelConverter<SocialEditPageModel, SocialEditPageViewModel>
    {
        private readonly ILocalizationModelService _localizationModelService;
        private readonly ISocialService<Entities.Social> _socialService;
        private readonly IUserTagService _userTagService;
        private readonly IUserTagProvider _userTagProvider;
        private readonly ILightboxHelper _lightboxHelper;
        private readonly IMediaService _mediaService;

        public SocialEditPageViewModelConverter(
            ILocalizationModelService localizationModelService,
            ISocialService<Entities.Social> socialService,
            IUserTagService userTagService,
            ILightboxHelper lightboxHelper, 
            IUserTagProvider userTagProvider, 
            IMediaService mediaService)
        {
            _localizationModelService = localizationModelService;
            _socialService = socialService;
            _userTagService = userTagService;
            _lightboxHelper = lightboxHelper;
            _userTagProvider = userTagProvider;
            _mediaService = mediaService;
        }

        public void Map(
            SocialEditPageModel node,
            SocialEditPageViewModel viewModel)
        {
            var id = HttpContext.Current.Request.GetRequestQueryValue("id");

            if (!Guid.TryParse(id, out var parsedId)) return;

            var social = _socialService.Get(parsedId);

            viewModel.OwnerId = social.OwnerId;
            viewModel.Id = social.Id; //TODO Use link service to navigate from social edit page
            viewModel.Description = social.Description;
            viewModel.Name = _localizationModelService["Social.Edit"];
            viewModel.Tags = _userTagService.Get(parsedId);
            viewModel.LightboxPreviewModel = _lightboxHelper.GetGalleryPreviewModel(social.MediaIds, PresetStrategies.ForActivityDetails);
            viewModel.AvailableTags = _userTagProvider.GetAll();
            viewModel.MediaRootId = _mediaService.GetRootMedia()
                .First(m => m.ContentType.Alias == "Folder" && m.Name == "Members Content").Id;
        }
    }
}