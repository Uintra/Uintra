using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Compent.CommandBus;
using Uintra.Core;
using Uintra.Core.Activity;
using Uintra.Core.Context;
using Uintra.Core.Extensions;
using Uintra.Core.User;
using Uintra.Likes.CommandBus;
using Umbraco.Web;
using static Uintra.Core.Context.Extensions.ContextExtensions;
using static Uintra.Core.Context.ContextType;

namespace Uintra.Likes.Web
{
    [TrackContext]
    public abstract class LikesControllerBase : ContextedController
    {
        protected virtual string LikesViewPath { get; set; } = "~/App_Plugins/Likes/View/LikesView.cshtml";

        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly ILikesService _likesService;
        private readonly ICommandPublisher _commandPublisher;

        protected LikesControllerBase(
            IActivitiesServiceFactory activitiesServiceFactory,
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            ILikesService likesService,
            IContextTypeProvider contextTypeProvider,
            ICommandPublisher commandPublisher)
            : base(contextTypeProvider)
        {
            _activitiesServiceFactory = activitiesServiceFactory;
            _intranetMemberService = intranetMemberService;
            _likesService = likesService;
            _commandPublisher = commandPublisher;
        }

        public virtual PartialViewResult ContentLikes()
        {
            var guid = CurrentPage.GetKey();
            return Likes(_likesService.GetLikeModels(guid), guid, showTitle: true);
        }

        public virtual PartialViewResult Likes(ILikeable likesInfo)
        {
            return Likes(likesInfo.Likes, likesInfo.Id, likesInfo.IsReadOnly);
        }

        public virtual PartialViewResult CommentLikes(Guid commentId)
        {
            return Likes(_likesService.GetLikeModels(commentId), commentId);
        }

        [HttpPost]
        [ContextAction(ContextBuildActionType.Add)]
        public virtual PartialViewResult AddLike()
        {
            var likeTarget = FullContext.Value;
            var targetEntityId = likeTarget.EntityId.Value;

            var command = new AddLikeCommand(FullContext, _intranetMemberService.GetCurrentMemberId());
            _commandPublisher.Publish(command);

            switch (likeTarget.Type.ToInt())
            {
                case (int)Comment:
                    return Likes(_likesService.GetLikeModels(targetEntityId), targetEntityId);

                case (int)ContentPage:
                    return Likes(_likesService.GetLikeModels(targetEntityId), targetEntityId, showTitle: true);

                case int type when HasFlagScalar(type, Activity | PagePromotion):
                    var activityLikeInfo = GetActivityLikes(targetEntityId);
                    return Likes(activityLikeInfo.Likes, activityLikeInfo.Id, activityLikeInfo.IsReadOnly, showTitle: HasFlagScalar(type, PagePromotion));
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        [HttpPost]
        [ContextAction(ContextBuildActionType.Remove)]
        public virtual PartialViewResult RemoveLike()
        {
            var likeTarget = FullContext.Value;
            var targetEntityId = likeTarget.EntityId.Value;

            var command = new RemoveLikeCommand(FullContext, _intranetMemberService.GetCurrentMemberId());
            _commandPublisher.Publish(command);

            switch (likeTarget.Type.ToInt())
            {
                case (int)Comment:
                    return Likes(_likesService.GetLikeModels(targetEntityId), targetEntityId);

                case (int)ContentPage:
                    return Likes(_likesService.GetLikeModels(targetEntityId), targetEntityId, showTitle: true);

                case int type when HasFlagScalar(type, Activity | PagePromotion):
                    var activityLikeInfo = GetActivityLikes(targetEntityId);
                    return Likes(activityLikeInfo.Likes, activityLikeInfo.Id, activityLikeInfo.IsReadOnly, showTitle: HasFlagScalar(type, PagePromotion));
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        protected virtual PartialViewResult Likes(IEnumerable<LikeModel> likes, Guid entityId, bool isReadOnly = false, bool showTitle = false)
        {
            var currenMemberId = _intranetMemberService.GetCurrentMemberId();
            var likeModels = likes as IList<LikeModel> ?? likes.ToList();
            var canAddLike = likeModels.All(el => el.UserId != currenMemberId);
            var model = new LikesViewModel
            {
                EntityId = entityId,
                MemberId = currenMemberId,
                Count = likeModels.Count,
                CanAddLike = canAddLike,
                Users = likeModels.Select(el => el.User),
                IsReadOnly = isReadOnly,
                ShowTitle = showTitle
            };
            return PartialView(LikesViewPath, model);
        }

        protected virtual ILikeable GetActivityLikes(Guid activityId)
        {
            var service = _activitiesServiceFactory.GetService<IIntranetActivityService<IIntranetActivity>>(activityId);
            return (ILikeable)service.Get(activityId);
        }
    }
}