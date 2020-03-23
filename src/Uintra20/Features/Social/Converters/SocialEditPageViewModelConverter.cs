using System;
using System.Web;
using Compent.Extensions;
using UBaseline.Core.Localization;
using UBaseline.Core.RequestContext;
using Uintra20.Core.Controls.LightboxGallery;
using Uintra20.Core.UbaselineModels.RestrictedNode;
using Uintra20.Features.Groups.Helpers;
using Uintra20.Features.Links;
using Uintra20.Features.Social.Models;
using Uintra20.Features.Media.Strategies.Preset;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Social.Converters
{
    public class SocialEditPageViewModelConverter
        : UintraRestrictedNodeViewModelConverter<SocialEditPageModel, SocialEditPageViewModel>
    {
        private readonly ILocalizationModelService _localizationModelService;
        private readonly ISocialService<Entities.Social> _socialService;
        private readonly IUserTagService _userTagService;
        private readonly IUserTagProvider _userTagProvider;
        private readonly ILightboxHelper _lightboxHelper;
        private readonly IFeedLinkService _feedLinkService;
        private readonly IGroupHelper _groupHelper;
        private readonly IUBaselineRequestContext _context;

        public SocialEditPageViewModelConverter(
            ILocalizationModelService localizationModelService,
            ISocialService<Entities.Social> socialService,
            IUserTagService userTagService,
            ILightboxHelper lightboxHelper,
            IUserTagProvider userTagProvider,
            IFeedLinkService feedLinkService,
            IGroupHelper groupHelper,
            IErrorLinksService errorLinksService,
            IUBaselineRequestContext context)
            : base(errorLinksService)
        {
            _localizationModelService = localizationModelService;
            _socialService = socialService;
            _userTagService = userTagService;
            _lightboxHelper = lightboxHelper;
            _userTagProvider = userTagProvider;
            _feedLinkService = feedLinkService;
            _groupHelper = groupHelper;
            _context = context;
        }

        public override ConverterResponseModel MapViewModel(SocialEditPageModel node, SocialEditPageViewModel viewModel)
        {
            var id = _context.ParseQueryString("id").TryParseGuid();

            if (!id.HasValue) return NotFoundResult();

            var social = _socialService.Get(id.Value);

            if (social == null)
            {
                return NotFoundResult();
            }

            if (!_socialService.CanEdit(id.Value))
            {
                return ForbiddenResult();
            }

            viewModel.CanDelete = _socialService.CanDelete(id.Value);
            viewModel.OwnerId = social.OwnerId;
            viewModel.Id = social.Id;
            viewModel.Description = social.Description;
            viewModel.Name = _localizationModelService["Social.Edit"];
            viewModel.Tags = _userTagService.Get(id.Value);
            viewModel.LightboxPreviewModel = _lightboxHelper.GetGalleryPreviewModel(social.MediaIds, PresetStrategies.ForActivityDetails);
            viewModel.AvailableTags = _userTagProvider.GetAll();
            viewModel.Links = _feedLinkService.GetLinks(social.Id);

            var mediaSettings = _socialService.GetMediaSettings();
            viewModel.AllowedMediaExtensions = mediaSettings.AllowedMediaExtensions;

            var groupId = HttpContext.Current.Request["groupId"].TryParseGuid();

            if (groupId.HasValue && social.GroupId == groupId.Value)
            {
                viewModel.GroupHeader = _groupHelper.GetHeader(groupId.Value);
            }

            return OkResult();
        }
    }
}