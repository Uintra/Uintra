using System;
using System.Linq;
using System.Web.Http;
using UBaseline.Core.Controllers;
using Uintra20.Features.Tagging.UserTags.Models;
using Uintra20.Features.Tagging.UserTags.Services;

namespace Uintra20.Features.Tagging.UserTags.Controllers
{
    public class UserTagsApiController : UBaselineApiController
    {
        private readonly IUserTagService _tagsService;
        private readonly IUserTagProvider _tagProvider;

        public UserTagsApiController(
            IUserTagService tagsService,
            IUserTagProvider tagProvider)
        {
            _tagsService = tagsService;
            _tagProvider = tagProvider;
        }

        [HttpGet]
        public TagsPickerViewModel GetTagsViewModel(Guid? entityId = null)
        {
            var pickerViewModel = new TagsPickerViewModel
            {
                UserTagCollection = _tagProvider.GetAll(),
                TagIdsData = entityId.HasValue
                    ? _tagsService.Get(entityId.Value).Select(t => t.Id)
                    : Enumerable.Empty<Guid>()
            };

            return pickerViewModel;
        }
    }
}