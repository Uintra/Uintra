using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using UBaseline.Core.Controllers;
using UBaseline.Core.Node;
using Uintra20.Features.Tagging.UserTags.Models;
using Uintra20.Features.Tagging.UserTags.Services;

namespace Uintra20.Features.Tagging.UserTags.Controllers
{
    public class UserTagsApiController : UBaselineApiController
    {
        private readonly IUserTagService _tagsService;
        private readonly IUserTagProvider _tagProvider;
        private readonly INodeModelService _modelService;

        public UserTagsApiController(
            IUserTagService tagsService,
            IUserTagProvider tagProvider,
            INodeModelService modelService)
        {
            _tagsService = tagsService;
            _tagProvider = tagProvider;
            _modelService = modelService;
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

        [HttpGet]
        public IEnumerable<UserTagPanelViewModel> GetAll(int pageId) =>
            _tagProvider.GetAll()
                .Select(tag => new UserTagPanelViewModel
                {
                    Id = tag.Id,
                    Name = tag.Text,
                });
    }
}