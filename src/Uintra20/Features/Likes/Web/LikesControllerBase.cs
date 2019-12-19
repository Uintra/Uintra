using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Compent.CommandBus;
using UBaseline.Core.Controllers;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Entities;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Likes.CommandBus.Commands;
using Uintra20.Features.Likes.Models;
using Uintra20.Features.Likes.Services;
using Uintra20.Infrastructure.Context;
using Uintra20.Infrastructure.Extensions;
using static Uintra20.Infrastructure.Context.ContextType;
using static Uintra20.Infrastructure.Extensions.ContextExtensions;

namespace Uintra20.Features.Likes.Web
{
    public abstract class LikesControllerBase : UBaselineApiController
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
        public virtual async Task<IEnumerable<LikeModel>> ContentLikes(Guid pageId)
        {
            return await _likesService.GetLikeModelsAsync(pageId);
        }

        //[HttpGet]
        //public virtual LikesViewModel Likes(ILikeable likesInfo)
        //{
        //    return likesInfo.Likes;
        //}

        [HttpGet]
        public virtual async Task<IEnumerable<LikeModel>> CommentLikes(Guid commentId)
        {
            return await _likesService.GetLikeModelsAsync(commentId);
        }

        [HttpPost]
        public virtual async Task<IEnumerable<LikeModel>> AddLike([FromUri]Guid entityId, [FromUri]ContextType entityType)
        {
            var command = new AddLikeCommand(entityId, entityType, await _intranetMemberService.GetCurrentMemberIdAsync());
            _commandPublisher.Publish(command);

            switch (entityType.ToInt())
            {
                case (int)Comment:
                    return await _likesService.GetLikeModelsAsync(entityId);

                case (int)ContentPage:
                    return await _likesService.GetLikeModelsAsync(entityId);

                case int type when HasFlagScalar(type, ContextType.Activity | PagePromotion):
                    var activityLikeInfo = GetActivityLikes(entityId);
                    return activityLikeInfo.Likes;
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        [HttpPost]
        public virtual async Task<IEnumerable<LikeModel>> RemoveLike(Guid entityId, ContextType entityType)
        {
            var command = new RemoveLikeCommand(entityId, entityType, await _intranetMemberService.GetCurrentMemberIdAsync());
            _commandPublisher.Publish(command);

            switch (entityType.ToInt())
            {
                case (int)Comment:
                    return await _likesService.GetLikeModelsAsync(entityId);

                case (int)ContentPage:
                    return await _likesService.GetLikeModelsAsync(entityId);

                case int type when HasFlagScalar(type, ContextType.Activity | PagePromotion):
                    var activityLikeInfo = GetActivityLikes(entityId);
                    return activityLikeInfo.Likes;
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

        protected virtual async Task<LikesViewModel> LikesAsync(IEnumerable<LikeModel> likes, Guid entityId, bool isReadOnly = false, bool showTitle = false)
        {
            var currenMemberId = await _intranetMemberService.GetCurrentMemberIdAsync();
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