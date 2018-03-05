using System;
using System.Linq;
using System.Web.Mvc;
using Compent.Uintra.Core.UserTags.ViewModels;
using Uintra.Core.Extensions;
using Uintra.Search;
using Uintra.Tagging.UserTags;
using Uintra.Tagging.UserTags.Models;
using Uintra.Tagging.Web;
using Umbraco.Web;

namespace Compent.Uintra.Controllers
{
    public class UserTagsController : UserTagsControllerBase
    {
        private readonly string EntityTagsViewPath = "~/Views/UserTags/EntityTags.cshtml";
        private readonly ISearchUmbracoHelper _searchUmbracoHelper;

        private readonly IUserTagService _tagsService;    

        public UserTagsController(
            IUserTagService tagsService,
            IUserTagProvider tagProvider,
            ISearchUmbracoHelper searchUmbracoHelper) : base(tagsService, tagProvider)
        {
            _tagsService = tagsService;
            _searchUmbracoHelper = searchUmbracoHelper;
        }

        public ActionResult ForEntity(Guid entityId)
        {
            var searchPage = _searchUmbracoHelper.GetSearchPage();
            var tags = _tagsService
                .Get(entityId)
                .Select(tag => MapToViewModel(tag, searchPage.Url))
                .ToList();
            return PartialView(EntityTagsViewPath, tags);
        }

        public ActionResult ForContent()
        {
            var currentContentPageId = CurrentPage.GetKey();
            return ForEntity(currentContentPageId);
        }

        private UserTagViewModel MapToViewModel(UserTag tag, string url)
        {
            return new UserTagViewModel()
            {
                Text = tag.Text,
                SearchUrl = url.AddQueryParameter(tag.Text)
            };
        }
    }
}