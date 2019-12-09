using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Compent.CommandBus;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Entities;
using Uintra20.Core.Member;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Likes.CommandBus.Commands;
using Uintra20.Features.Likes.Models;
using Uintra20.Features.Likes.Services;
using Uintra20.Infrastructure.Context;
using Uintra20.Infrastructure.Extensions;
using Umbraco.Web.WebApi;
using static Uintra20.Infrastructure.Context.ContextType;
using static Uintra20.Infrastructure.Extensions.ContextExtensions;

namespace Uintra20.Features.Likes.Web
{
    public abstract class LikesControllerBase : UmbracoApiController
    {
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly ILikesService _likesService;
        private readonly ICommandPublisher _commandPublisher;

        protected LikesControllerBase(
            IActivitiesServiceFactory activitiesServiceFactory,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            ILikesService likesService,
            ICommandPublisher commandPublisher)
        {
            _activitiesServiceFactory = activitiesServiceFactory;
            _intranetMemberService = intranetMemberService;
            _likesService = likesService;
            _commandPublisher = commandPublisher;
        }

        [HttpGet]
        public virtual LikesViewModel ContentLikes(Guid pageId)
        {
            return Likes(_likesService.GetLikeModels(pageId), pageId, showTitle: true);
        }

        [HttpGet]
        public virtual LikesViewModel Likes(ILikeable likesInfo)
        {
            return Likes(likesInfo.Likes, likesInfo.Id, likesInfo.IsReadOnly);
        }

        [HttpGet]
        public virtual LikesViewModel CommentLikes(Guid commentId)
        {
            return Likes(_likesService.GetLikeModels(commentId), commentId);
        }

        [HttpPost]
        public virtual LikesViewModel AddLike(Guid entityId, ContextType entityType)
        {
            var command = new AddLikeCommand(entityId, entityType, _intranetMemberService.GetCurrentMemberId());
            _commandPublisher.Publish(command);

            switch (entityType.ToInt())
            {
                case (int)Comment:
                    return Likes(_likesService.GetLikeModels(entityId), entityId);

                case (int)ContentPage:
                    return Likes(_likesService.GetLikeModels(entityId), entityId, showTitle: true);

                case int type when HasFlagScalar(type, ContextType.Activity | PagePromotion):
                    var activityLikeInfo = GetActivityLikes(entityId);
                    return Likes(activityLikeInfo.Likes, activityLikeInfo.Id, activityLikeInfo.IsReadOnly, showTitle: HasFlagScalar(type, PagePromotion));
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        [HttpPost]
        public virtual LikesViewModel RemoveLike(Guid entityId, ContextType entityType)
        {
            var command = new RemoveLikeCommand(entityId, entityType, _intranetMemberService.GetCurrentMemberId());
            _commandPublisher.Publish(command);

            switch (entityType.ToInt())
            {
                case (int)Comment:
                    return Likes(_likesService.GetLikeModels(entityId), entityId);

                case (int)ContentPage:
                    return Likes(_likesService.GetLikeModels(entityId), entityId, showTitle: true);

                case int type when HasFlagScalar(type, ContextType.Activity | PagePromotion):
                    var activityLikeInfo = GetActivityLikes(entityId);
                    return Likes(activityLikeInfo.Likes, activityLikeInfo.Id, activityLikeInfo.IsReadOnly, showTitle: HasFlagScalar(type, PagePromotion));
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        protected virtual LikesViewModel Likes(IEnumerable<LikeModel> likes, Guid entityId, bool isReadOnly = false, bool showTitle = false)
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
            return model;
        }

        protected virtual ILikeable GetActivityLikes(Guid activityId)
        {
            var service = _activitiesServiceFactory.GetService<IIntranetActivityService<IIntranetActivity>>(activityId);
            return (ILikeable)service.Get(activityId);
        }
    }
}