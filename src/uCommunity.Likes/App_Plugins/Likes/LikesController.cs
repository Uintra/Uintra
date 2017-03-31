using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uCommunity.Core.Activity;
using uCommunity.Core.User;
using Umbraco.Web.Mvc;

namespace uCommunity.Likes
{
    public class LikesController : SurfaceController
    {
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly IIntranetUserService _intranetUserService;


        public LikesController(
            IActivitiesServiceFactory activitiesServiceFactory,
            IIntranetUserService intranetUserService)
        {
            _activitiesServiceFactory = activitiesServiceFactory;
            _intranetUserService = intranetUserService;
        }

        public PartialViewResult Likes(ILikeable likesInfo)
        {
            return Likes(likesInfo.Id, likesInfo.Likes);
        }

        [HttpPost]
        public PartialViewResult AddLike(Guid activityId)
        {
            var service = _activitiesServiceFactory.GetService(activityId);
            var likeableService = (ILikeableService)service;
            var likeInfo = likeableService.Add(GetCurrentUserId(), activityId);
            
            return Likes(likeInfo.Id, likeInfo.Likes);
        }

        [HttpPost]
        public PartialViewResult RemoveLike(Guid activityId)
        {
            var service = _activitiesServiceFactory.GetService(activityId);
            var likeableService = (ILikeableService)service;
            var likeInfo = likeableService.Remove(GetCurrentUserId(), activityId);

            return Likes(likeInfo.Id, likeInfo.Likes);
        }

        private PartialViewResult Likes(Guid activityId, IEnumerable<LikeModel> likes)
        {
            var currentUserId = GetCurrentUserId();

            var canAddLike = !likes.Any(el => el.UserId == currentUserId);
            var model = new LikesViewModel
            {
                ActivityId = activityId,
                UserId = currentUserId,
                Count = likes.Count(),
                CanAddLike = canAddLike,
                Users = likes.Select(el => el.User)
            };

            return PartialView("~/App_Plugins/Likes/View/LikesView.cshtml", model);
        }

        private Guid GetCurrentUserId()
        {
            return _intranetUserService.GetCurrentUserId();
        }
    }
}