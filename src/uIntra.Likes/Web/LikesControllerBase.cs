using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uIntra.Core;
using uIntra.Core.Activity;
using uIntra.Core.Extensions;
using uIntra.Core.PagePromotion;
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
            IDocumentTypeAliasProvider documentTypeAliasProvider,
            UmbracoHelper umbracoHelper)
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
            return Likes(_likesService.GetLikeModels(guid), guid, showTitle: true);
        }

        public virtual PartialViewResult Likes(ILikeable likesInfo)
        {
            return Likes(likesInfo.Likes, likesInfo.Id, isReadOnly: likesInfo.IsReadOnly);
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

            if (IsForPagePromotion(model))
            {
                var pagePromotionLikeInfo = AddActivityLike(model.ActivityId);
                return Likes(pagePromotionLikeInfo.Likes, pagePromotionLikeInfo.Id, showTitle: true);
            }

            if (IsForContentPage(model))
            {
                _likesService.Add(GetCurrentUserId(), model.ActivityId);
                return Likes(_likesService.GetLikeModels(model.ActivityId), model.ActivityId);
            }

            var activityLikeInfo = AddActivityLike(model.ActivityId);
            return Likes(activityLikeInfo.Likes, activityLikeInfo.Id);
        }

        [HttpPost]
        public virtual PartialViewResult RemoveLike(AddRemoveLikeModel model)
        {
            if (IsForComment(model))
            {
                _likesService.Remove(GetCurrentUserId(), model.CommentId.Value);
                return Likes(_likesService.GetLikeModels(model.CommentId.Value), model.ActivityId, model.CommentId);
            }

            if (IsForPagePromotion(model))
            {
                var pagePromotionLikeInfo = RemoveActivityLike(model.ActivityId);
                return Likes(pagePromotionLikeInfo.Likes, pagePromotionLikeInfo.Id, showTitle: true);
            }

            if (IsForContentPage(model))
            {
                _likesService.Remove(GetCurrentUserId(), model.ActivityId);
                return Likes(_likesService.GetLikeModels(model.ActivityId), model.ActivityId);
            }

            var activityLikeInfo = RemoveActivityLike(model.ActivityId);
            return Likes(activityLikeInfo.Likes, activityLikeInfo.Id);
        }

        protected virtual bool IsForComment(AddRemoveLikeModel model)
        {
            return model.CommentId.HasValue;
        }

        protected virtual bool IsForPagePromotion(AddRemoveLikeModel model)
        {
            var content = _umbracoHelper.TypedContent(model.ActivityId);
            return content != null && PagePromotionHelper.IsPagePromotion(content);
        }

        protected virtual bool IsForContentPage(AddRemoveLikeModel model)
        {
            return _umbracoHelper.TypedContent(model.ActivityId)?.DocumentTypeAlias == _documentTypeAliasProvider.GetContentPage();
        }

        protected virtual PartialViewResult Likes(IEnumerable<LikeModel> likes, Guid activityId, Guid? commentId = null, bool isReadOnly = false, bool showTitle = false)
        {
            var currentUserId = GetCurrentUserId();
            var likeModels = likes as IList<LikeModel> ?? likes.ToList();
            var canAddLike = likeModels.All(el => el.UserId != currentUserId);
            var model = new LikesViewModel
            {
                ActivityId = activityId,
                CommentId = commentId,
                UserId = currentUserId,
                Count = likeModels.Count,
                CanAddLike = canAddLike,
                Users = likeModels.Select(el => el.User),
                IsReadOnly = isReadOnly,
                ShowTitle = showTitle
            };
            return PartialView(LikesViewPath, model);
        }

        protected virtual Guid GetCurrentUserId() => _intranetUserService.GetCurrentUserId();

        protected ILikeable AddActivityLike(Guid activityId)
        {
            var service = _activitiesServiceFactory.GetService<ILikeableService>(activityId);
            return service.AddLike(GetCurrentUserId(), activityId);
        }

        protected ILikeable RemoveActivityLike(Guid activityId)
        {
            var service = _activitiesServiceFactory.GetService<ILikeableService>(activityId);
            return service.RemoveLike(GetCurrentUserId(), activityId);
        }
    }
}