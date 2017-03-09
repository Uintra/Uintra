using System;
using System.Linq;
using System.Web.Mvc;
using uCommunity.Core.App_Plugins.Core.User;
using uCommunity.Likes.App_Plugins.Likes;
using uCommunity.Likes.App_Plugins.Likes.Models;
using Umbraco.Web.Mvc;

namespace uCommunity.Likes.Controllers
{
    public class LikesController : SurfaceController
    {
        private readonly ILikesService _likesService;
        private readonly IIntranetUserService _intranetUserService;

        public LikesController(
            ILikesService likesService,
            IIntranetUserService intranetUserService)
        {
            _likesService = likesService;
            _intranetUserService = intranetUserService;
        }

        public PartialViewResult Likes(Guid activityId)
        {
            return PartialView("~/App_Plugins/Likes/View/LikesView.cshtml", GetLikesModel(activityId));
        }

        [HttpPost]
        public PartialViewResult AddLike(Guid activityId)
        {
            _likesService.Add(GetCurrentUserId(), activityId);

            return PartialView("~/App_Plugins/Likes/View/LikesView.cshtml", GetLikesModel(activityId));
        }

        [HttpPost]
        public PartialViewResult RemoveLike(Guid activityId)
        {
            _likesService.Remove(GetCurrentUserId(), activityId);

            return PartialView("~/App_Plugins/Likes/View/LikesView.cshtml", GetLikesModel(activityId));
        }

        private LikesViewModel GetLikesModel(Guid activityId)
        {
            var currentUserId = GetCurrentUserId();
            var likes = _likesService.Get(activityId).OrderByDescending(like => like.CreatedDate).ToList();
            var userNames = likes.Count > 0
                ? _intranetUserService.GetFullNamesByIds(likes.Select(el => el.UserId))
                : Enumerable.Empty<string>();
            var canAddLike = _likesService.CanAdd(currentUserId, activityId);

            var likesViewModel = new LikesViewModel()
            {
                ActivityId = activityId,
                UserId = currentUserId,
                Count = likes.Count,
                CanAddLike = canAddLike,
                CanRemoveLike = !canAddLike,
                Users = userNames
            };

            return likesViewModel;
        }

        private Guid GetCurrentUserId()
        {
            return _intranetUserService.GetCurrentUserId();
        }
    }
}