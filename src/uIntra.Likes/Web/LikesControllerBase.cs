using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uIntra.Core;
using uIntra.Core.Activity;
using uIntra.Core.Extentions;
using uIntra.Core.User;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace uIntra.Likes.Web
{
    public abstract class LikesControllerBase : SurfaceController
    {
        protected virtual string LikesViewPath { get; set; } = "~/App_Plugins/Likes/View/LikesView.cshtml";

        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly ILikesService _likesService;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly UmbracoHelper _umbracoHelper;

        protected LikesControllerBase(
            IActivitiesServiceFactory activitiesServiceFactory,
            IIntranetUserService<IIntranetUser> intranetUserService,
            ILikesService likesService,
            IDocumentTypeAliasProvider documentTypeAliasProvider, UmbracoHelper umbracoHelper)
        {
            _activitiesServiceFactory = activitiesServiceFactory;
            _intranetUserService = intranetUserService;
            _likesService = likesService;
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _umbracoHelper = umbracoHelper;
        }

        public virtual PartialViewResult ContentLikes()
        {
            var guid = CurrentPage.GetGuidKey();
            return Likes(_likesService.GetLikeModels(guid), guid);
        }

        public virtual PartialViewResult Likes(ILikeable likesInfo)
        {
            return Likes(likesInfo.Likes, likesInfo.Id);
        }

        public virtual PartialViewResult CommentLikes(Guid activityId, Guid commentId)
        {
            return Likes(_likesService.GetLikeModels(commentId), activityId, commentId);
        }

        [HttpPost]
        public virtual PartialViewResult AddLike(AddRemoveLikeModel model)
        {
            if (IsForComment(model))
            {
                _likesService.Add(GetCurrentUserId(), model.CommentId.Value);
                return Likes(_likesService.GetLikeModels(model.CommentId.Value), model.ActivityId, model.CommentId);
            }

            if (IsForContentPage(model))
            {
                _likesService.Add(GetCurrentUserId(), model.ActivityId);
                return Likes(_likesService.GetLikeModels(model.ActivityId), model.ActivityId);
            }

            return AddActivityLike(model.ActivityId);
        }

        [HttpPost]
        public virtual PartialViewResult RemoveLike(AddRemoveLikeModel model)
        {
            if (IsForComment(model))
            {
                _likesService.Remove(GetCurrentUserId(), model.CommentId.Value);
                return Likes(_likesService.GetLikeModels(model.CommentId.Value), model.ActivityId, model.CommentId);
            }

            if (IsForContentPage(model))
            {
                _likesService.Remove(GetCurrentUserId(), model.ActivityId);
                return Likes(_likesService.GetLikeModels(model.ActivityId), model.ActivityId);
            }

            return RemoveActivityLike(model.ActivityId);
        }

        protected virtual bool IsForComment(AddRemoveLikeModel model)
        {
            return model.CommentId.HasValue;
        }

        protected virtual bool IsForContentPage(AddRemoveLikeModel model)
        {
            return _umbracoHelper.TypedContent(model.ActivityId)?.DocumentTypeAlias == _documentTypeAliasProvider.GetContentPage();
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
            var service = _activitiesServiceFactory.GetService<ILikeableService>(activityId);
            var likeInfo = service.AddLike(GetCurrentUserId(), activityId);

            return Likes(likeInfo.Likes, likeInfo.Id);
        }

        protected virtual PartialViewResult RemoveActivityLike(Guid activityId)
        {
            var service = _activitiesServiceFactory.GetService<ILikeableService>(activityId);
            var likeInfo = service.RemoveLike(GetCurrentUserId(), activityId);

            return Likes(likeInfo.Likes, likeInfo.Id);
        }

        protected virtual Guid GetCurrentUserId()
        {
            return _intranetUserService.GetCurrentUserId();
        }
    }
}