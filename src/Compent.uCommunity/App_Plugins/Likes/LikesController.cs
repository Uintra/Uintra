using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ServiceStack.OrmLite;
using uCommunity.Core.Activity;
using uCommunity.Core.User;
using Umbraco.Web.Mvc;

namespace uCommunity.Likes.Controllers
{
    public class LikesController : SurfaceController
    {
        //private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly IIntranetUserService _intranetUserService;
        private readonly ILikeableService likeableService;
        

        public LikesController(
            //IActivitiesServiceFactory activitiesServiceFactory,
            IIntranetUserService intranetUserService,
            ILikeableService likeableService)
        {
            //_activitiesServiceFactory = activitiesServiceFactory;
            _intranetUserService = intranetUserService;
            this.likeableService = likeableService;
        }

        public PartialViewResult Likes(ILikeable likesInfo)
        {
            return PartialView("~/App_Plugins/Likes/View/LikesView.cshtml", GetLikesModel(likesInfo.Id, likesInfo.Type, likesInfo.Likes));
        }

        [HttpPost]
        public PartialViewResult AddLike(Guid activityId, IntranetActivityTypeEnum type)
        {
            //var service = _activitiesServiceFactory.GetService(type);
            //var likeableService = (ILikeableService)service;
            likeableService.Add(GetCurrentUserId(), activityId);
            return PartialView("~/App_Plugins/Likes/View/LikesView.cshtml", GetLikesModel(activityId, type, likeableService.GetLikes(activityId)));
        }

        //[HttpPost]
        public PartialViewResult RemoveLike(Guid activityId, IntranetActivityTypeEnum type)
        {
            //var service = _activitiesServiceFactory.GetService(type);
            //var likeableService = (ILikeableService)service;
            likeableService.Remove(GetCurrentUserId(), activityId);

            return PartialView("~/App_Plugins/Likes/View/LikesView.cshtml", GetLikesModel(activityId, type, likeableService.GetLikes(activityId)));
        }

        private LikesViewModel GetLikesModel(Guid activityId, IntranetActivityTypeEnum type, IEnumerable<LikeModel> likes)
        {
            var currentUserId = GetCurrentUserId();

            var canAddLike = !likes.Any(el => el.UserId == currentUserId);

            var likesViewModel = new LikesViewModel()
            {
                ActivityId = activityId,
                UserId = currentUserId,
                Count = likes.Count(),
                CanAddLike = canAddLike,
                CanRemoveLike = !canAddLike,
                Users = likes.Select(el => el.User),
                Type = type
            };

            return likesViewModel;
        }

        private Guid GetCurrentUserId()
        {
            return _intranetUserService.GetCurrentUserId();
        }
    }
}