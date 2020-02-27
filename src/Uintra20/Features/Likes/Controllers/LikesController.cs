using Compent.CommandBus;
using Localization.Umbraco.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using UBaseline.Core.Controllers;
using Uintra20.Attributes;
using Uintra20.Core;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Entities;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Likes.CommandBus.Commands;
using Uintra20.Features.Likes.Models;
using Uintra20.Features.Likes.Services;
using Uintra20.Infrastructure.Context;
using Uintra20.Infrastructure.Extensions;
using static Uintra20.Infrastructure.Extensions.ContextExtensions;

namespace Uintra20.Features.Likes.Controllers
{
    [ThreadCulture]
    [ValidateModel]
    public class LikesController : UBaselineApiController
    {
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly ILikesService _likesService;
        private readonly ICommandPublisher _commandPublisher;

        public LikesController(
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
        public async Task<IEnumerable<LikeModel>> ContentLikes(Guid pageId)
        {
            return await _likesService.GetLikeModelsAsync(pageId);
        }

        //[HttpGet]
        //public virtual LikesViewModel Likes(ILikeable likesInfo)
        //{
        //    return likesInfo.Likes;
        //}

        [HttpGet]
        public async Task<IEnumerable<LikeModel>> CommentLikes(Guid commentId)
        {
            return await _likesService.GetLikeModelsAsync(commentId);
        }

        [HttpPost]
        public async Task<IEnumerable<LikeModel>> AddLike([FromUri]Guid entityId, [FromUri]IntranetEntityTypeEnum entityType)
        {
            var command = new AddLikeCommand(entityId, entityType, await _intranetMemberService.GetCurrentMemberIdAsync());
            _commandPublisher.Publish(command);

            switch (entityType)
            {
                case IntranetEntityTypeEnum.Comment:
                    return await _likesService.GetLikeModelsAsync(entityId);

                case IntranetEntityTypeEnum.ContentPage:
                    return await _likesService.GetLikeModelsAsync(entityId);

                case IntranetEntityTypeEnum type when type.Is(IntranetEntityTypeEnum.News, IntranetEntityTypeEnum.Social, IntranetEntityTypeEnum.Events):
                    var activityLikeInfo = GetActivityLikes(entityId);
                    return activityLikeInfo.Likes;
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        [HttpPost]
        public async Task<IEnumerable<LikeModel>> RemoveLike(Guid entityId, IntranetEntityTypeEnum entityType)
        {
            var command = new RemoveLikeCommand(entityId, entityType, await _intranetMemberService.GetCurrentMemberIdAsync());
            _commandPublisher.Publish(command);

            switch (entityType)
            {
                case IntranetEntityTypeEnum.Comment:
                    return await _likesService.GetLikeModelsAsync(entityId);

                case IntranetEntityTypeEnum.ContentPage:
                    return await _likesService.GetLikeModelsAsync(entityId);

                case IntranetEntityTypeEnum type when type.Is(IntranetEntityTypeEnum.News, IntranetEntityTypeEnum.Social, IntranetEntityTypeEnum.Events):
                    var activityLikeInfo = GetActivityLikes(entityId);
                    return activityLikeInfo.Likes;
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        private LikesViewModel Likes(IEnumerable<LikeModel> likes, Guid entityId, bool isReadOnly = false, bool showTitle = false)
        {
            var currenMemberId = _intranetMemberService.GetCurrentMemberId();
            var likeModels = likes.ToArray();
            var likedByCurrentUser = likeModels.Any(el => el.UserId == currenMemberId);
            var model = new LikesViewModel
            {
                EntityId = entityId,
                MemberId = currenMemberId,
                Count = likeModels.Length,
                LikedByCurrentUser = likedByCurrentUser,
                Users = likeModels.Select(el => el.User),
                IsReadOnly = isReadOnly,
                ShowTitle = showTitle
            };
            return model;
        }

        private async Task<LikesViewModel> LikesAsync(IEnumerable<LikeModel> likes, Guid entityId, bool isReadOnly = false, bool showTitle = false)
        {
            var currenMemberId = await _intranetMemberService.GetCurrentMemberIdAsync();
            var likeModels = likes.ToArray();
            var likedByCurrentUser = likeModels.Any(el => el.UserId == currenMemberId);
            var model = new LikesViewModel
            {
                EntityId = entityId,
                MemberId = currenMemberId,
                Count = likeModels.Length,
                LikedByCurrentUser = likedByCurrentUser,
                Users = likeModels.Select(el => el.User),
                IsReadOnly = isReadOnly,
                ShowTitle = showTitle
            };
            return model;
        }

        private ILikeable GetActivityLikes(Guid activityId)
        {
            var service = _activitiesServiceFactory.GetService<IIntranetActivityService<IIntranetActivity>>(activityId);
            return (ILikeable)service.Get(activityId);
        }
    }
}