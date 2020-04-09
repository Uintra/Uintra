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
        public IEnumerable<UserTagBackofficeViewModel> GetAll(int pageId)
        {
            var nodeModel = _modelService.Get(pageId);

            var allTags = _tagProvider.GetAll();

            var selectedTagsDictionary = _tagsService
                .Get(nodeModel.Key)
                .ToDictionary(tag => tag.Id);

            var result = allTags.Select(tag => new UserTagBackofficeViewModel
            {
                Id = tag.Id,
                Text = tag.Text,
                Selected = selectedTagsDictionary.ContainsKey(tag.Id)
            });

            return result;
        }
    }
}