using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uCommunity.Core;
using uCommunity.Core.Activity;
using uCommunity.Core.User;
using Umbraco.Web.Mvc;

namespace uCommunity.Likes.Web
{
    public abstract class LikesControllerBase : SurfaceController
    {
        public virtual string LikesViewPath { get; set; } = "~/App_Plugins/Likes/View/LikesView.cshtml";

        protected readonly IActivitiesServiceFactory ActivitiesServiceFactory;
        protected readonly IIntranetUserService IntranetUserService;
        protected readonly ILikesService LikesService;

        protected LikesControllerBase(
            IActivitiesServiceFactory activitiesServiceFactory,
            IIntranetUserService intranetUserService,
            ILikesService likesService)
        {
            ActivitiesServiceFactory = activitiesServiceFactory;
            IntranetUserService = intranetUserService;
            LikesService = likesService;
        }

        public virtual PartialViewResult Likes(ILikeable likesInfo)
        {
            return Likes(likesInfo.Likes, likesInfo.Id);
        }

        public virtual PartialViewResult CommentLikes(Guid activityId, Guid commentId)
        {
            return Likes(LikesService.GetLikeModels(commentId), activityId, commentId);
        }

        [HttpPost]
        public virtual PartialViewResult AddLike(AddRemoveLikeModel model)
        {
            if (model.CommentId.HasValue)
            {
                LikesService.Add(GetCurrentUserId(), model.CommentId.Value);
                return Likes(LikesService.GetLikeModels(model.CommentId.Value), model.ActivityId, model.CommentId);
            }

            return AddActivityLike(model.ActivityId);
        }

        [HttpPost]
        public virtual PartialViewResult RemoveLike(AddRemoveLikeModel model)
        {
            if (model.CommentId.HasValue)
            {
                LikesService.Remove(GetCurrentUserId(), model.CommentId.Value);
                return Likes(LikesService.GetLikeModels(model.CommentId.Value), model.ActivityId, model.CommentId);
            }

            return RemoveActivityLike(model.ActivityId);
        }

        protected virtual PartialViewResult Likes(IEnumerable<LikeModel> likes, Guid activityId, Guid? commentId = null)
        {
            var currentUserId = GetCurrentUserId();

            var canAddLike = !likes.Any(el => el.UserId == currentUserId);
            var model = new LikesViewModel
            {
                ActivityId = activityId,
                CommentId = commentId,
                UserId = currentUserId,
                Count = likes.Count(),
                CanAddLike = canAddLike,
                Users = likes.Select(el => el.User)
            };

            return PartialView(LikesViewPath, model);
        }

        protected virtual PartialViewResult AddActivityLike(Guid activityId)
        {
            var service = ActivitiesServiceFactory.GetService(activityId);
            var likeableService = (ILikeableService)service;
            var likeInfo = likeableService.Add(GetCurrentUserId(), activityId);

            return Likes(likeInfo.Likes, likeInfo.Id);
        }

        protected virtual PartialViewResult RemoveActivityLike(Guid activityId)
        {
            var service = ActivitiesServiceFactory.GetService(activityId);
            var likeableService = (ILikeableService)service;
            var likeInfo = likeableService.Remove(GetCurrentUserId(), activityId);

            return Likes(likeInfo.Likes, likeInfo.Id);
        }

        protected virtual Guid GetCurrentUserId()
        {
            return IntranetUserService.GetCurrentUserId();
        }
    }
}