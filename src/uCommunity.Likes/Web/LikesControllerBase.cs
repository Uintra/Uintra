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

        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly IIntranetUserService _intranetUserService;
        private readonly ILikesService _likesService;

        protected LikesControllerBase(
            IActivitiesServiceFactory activitiesServiceFactory,
            IIntranetUserService intranetUserService,
            ILikesService likesService)
        {
            _activitiesServiceFactory = activitiesServiceFactory;
            _intranetUserService = intranetUserService;
            _likesService = likesService;
        }

        public virtual PartialViewResult Likes(ILikeable likesInfo)
        {
            return Likes(likesInfo.Likes, activityId: likesInfo.Id);
        }

        public virtual PartialViewResult CommentLikes(Guid commentId)
        {
            return Likes(_likesService.GetLikeModels(commentId), commentId: commentId);
        }

        [HttpPost]
        public virtual PartialViewResult AddLike(AddRemoveLikeModel model)
        {
            if (model.ActivityId.HasValue)
            {
                return AddActivityLike(model.ActivityId.Value);
            }

            if (model.CommentId.HasValue)
            {
                _likesService.Add(GetCurrentUserId(), model.CommentId.Value);
                return Likes(_likesService.GetLikeModels(model.CommentId.Value), commentId: model.CommentId.Value);
            }

            throw new ArgumentException("Error while adding comment. ActivityId or CommentId should have value.");
        }

        [HttpPost]
        public virtual PartialViewResult RemoveLike(AddRemoveLikeModel model)
        {
            if (model.ActivityId.HasValue)
            {
                return RemoveActivityLike(model.ActivityId.Value);
            }

            if (model.CommentId.HasValue)
            {
                _likesService.Remove(GetCurrentUserId(), model.CommentId.Value);
                return Likes(_likesService.GetLikeModels(model.CommentId.Value), commentId: model.CommentId.Value);
            }

            throw new ArgumentException("Error while removing comment. ActivityId or CommentId should have value.");
        }

        protected virtual PartialViewResult Likes(IEnumerable<LikeModel> likes, Guid? activityId = null, Guid? commentId = null)
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
            var service = _activitiesServiceFactory.GetService(activityId);
            var likeableService = (ILikeableService)service;
            var likeInfo = likeableService.Add(GetCurrentUserId(), activityId);

            return Likes(likeInfo.Likes, activityId: likeInfo.Id);
        }

        protected virtual PartialViewResult RemoveActivityLike(Guid activityId)
        {
            var service = _activitiesServiceFactory.GetService(activityId);
            var likeableService = (ILikeableService)service;
            var likeInfo = likeableService.Remove(GetCurrentUserId(), activityId);

            return Likes(likeInfo.Likes, activityId: likeInfo.Id);
        }

        protected virtual Guid GetCurrentUserId()
        {
            return _intranetUserService.GetCurrentUserId();
        }
    }
}