using System;
using System.Linq;
using System.Web.Mvc;
using Compent.uIntra.Core.UserTags.ViewModels;
using uIntra.Tagging.UserTags;
using uIntra.Tagging.UserTags.Models;
using uIntra.Tagging.Web;
using Umbraco.Web;

namespace Compent.uIntra.Controllers
{
    public class UserTagsController : UserTagsControllerBase
    {
        private readonly string EntityTagsViewPath = "~/Views/UserTags/EntityTags.cshtml";

        private readonly IUserTagService _tagsService;    

        public UserTagsController(IUserTagService tagsService, IUserTagProvider tagProvider) : base(tagsService, tagProvider)
        {
            _tagsService = tagsService;
        }

        public ActionResult ForEntity(Guid entityId)
        {
            var tags = _tagsService
                .GetRelatedTags(entityId)
                .Select(MapToViewModel)
                .ToList();
            return PartialView(EntityTagsViewPath, tags);
        }

        public ActionResult ForContent()
        {
            var currentContentPageId = CurrentPage.GetKey();
            return ForEntity(currentContentPageId);
        }

        private UserTagViewModel MapToViewModel(UserTag tag)
        {
            return new UserTagViewModel()
            {
                Text = tag.Text,
                SearchUrl = "potato"
            };
        }
    }
}