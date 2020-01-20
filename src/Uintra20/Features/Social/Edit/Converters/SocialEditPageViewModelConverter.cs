using System;
using System.Web;
using UBaseline.Core.Node;
using Uintra20.Core.Controls.LightboxGallery;
using Uintra20.Features.Media.Strategies.ImageResize;
using Uintra20.Features.Social.Edit.Models;
using Uintra20.Features.Tagging.UserTags.Models;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Social.Edit.Converters
{
    public class SocialEditPageViewModelConverter
        : INodeViewModelConverter<SocialEditPageModel, SocialEditPageViewModel>
    {
        private readonly ISocialService<Entities.Social> _socialService;
        private readonly IUserTagService _userTagService;
        private readonly IUserTagProvider _userTagProvider;
        private readonly ILightboxHelper _lightboxHelper;

        public SocialEditPageViewModelConverter(
            ISocialService<Entities.Social> socialService,
            IUserTagService userTagService,
            ILightboxHelper lightboxHelper, 
            IUserTagProvider userTagProvider)
        {
            _socialService = socialService;
            _userTagService = userTagService;
            _lightboxHelper = lightboxHelper;
            _userTagProvider = userTagProvider;
        }

        public void Map(
            SocialEditPageModel node,
            SocialEditPageViewModel viewModel)
        {
            var id = HttpContext.Current.Request.GetUbaselineQueryValue("id");

            if (!Guid.TryParse(id, out var parsedId)) return;

            var social = _socialService.Get(parsedId);

            viewModel.ActivityId = social.Id; //TODO Use link service to navigate from social edit page
            viewModel.Description = social.Description;
            viewModel.Name = "Edit Social";
            viewModel.Tags = _userTagService.Get(parsedId);
            viewModel.LightboxPreviewModel = _lightboxHelper.GetGalleryPreviewModel(social.MediaIds, RenderStrategies.ForActivityDetails);
            viewModel.AvailableTags = _userTagProvider.GetAll();
        }
    }
}