using Compent.CommandBus;
using Localization.Umbraco.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using UBaseline.Core.Controllers;
using Uintra20.Attributes;
using Uintra20.Core;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Entities;
using Uintra20.Core.Activity.Helpers;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Comments.Services;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Likes.CommandBus.Commands;
using Uintra20.Features.Likes.Models;
using Uintra20.Features.Likes.Services;
using Uintra20.Infrastructure.Extensions;

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
        private readonly IGroupActivityService _groupActivityService;
        private readonly ICommentsService _commentsService;
        private readonly IActivityTypeHelper _activityTypeHelper;

        public LikesController(
            IActivitiesServiceFactory activitiesServiceFactory,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            ILikesService likesService,
            ICommandPublisher commandPublisher,
            IGroupActivityService groupActivityService,
            ICommentsService commentsService,
            IActivityTypeHelper activityTypeHelper)
        {
            _activitiesServiceFactory = activitiesServiceFactory;
            _intranetMemberService = intranetMemberService;
            _likesService = likesService;
            _commandPublisher = commandPublisher;
            _groupActivityService = groupActivityService;
            _commentsService = commentsService;
            _activityTypeHelper = activityTypeHelper;
        }

        [HttpGet]
        public async Task<IEnumerable<LikeModel>> ContentLikes(Guid pageId)
        {
            return await _likesService.GetLikeModelsAsync(pageId);
        }

        [HttpGet]
        public async Task<IEnumerable<LikeModel>> CommentLikes(Guid commentId)
        {
            return await _likesService.GetLikeModelsAsync(commentId);
        }

        [HttpPost]
        public async Task<IHttpActionResult> AddLike([FromUri]Guid entityId, [FromUri]IntranetEntityTypeEnum entityType)
        {
            if (!await CanAddLikeAsync(entityId, entityType))
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            var command = new AddLikeCommand(entityId, entityType, await _intranetMemberService.GetCurrentMemberIdAsync());
            _commandPublisher.Publish(command);

            switch (entityType)
            {
                case IntranetEntityTypeEnum.Comment:
                    return Ok(await _likesService.GetLikeModelsAsync(entityId));

                case IntranetEntityTypeEnum.ContentPage:
                    return Ok(await _likesService.GetLikeModelsAsync(entityId));

                case IntranetEntityTypeEnum type when type.Is(IntranetEntityTypeEnum.News, IntranetEntityTypeEnum.Social, IntranetEntityTypeEnum.Events):
                    var activityLikeInfo = GetActivityLikes(entityId);
                    return Ok(activityLikeInfo.Likes);
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

        private async Task<bool> CanAddLikeAsync(Guid entityId, IntranetEntityTypeEnum entityType)
        {
            if (entityType.Is(IntranetEntityTypeEnum.Social, IntranetEntityTypeEnum.News, IntranetEntityTypeEnum.Events))
            {
                var member = await _intranetMemberService.GetCurrentMemberAsync();
                var activityGroupId = _groupActivityService.GetGroupId(entityId);

                if (activityGroupId.HasValue && !member.GroupIds.Contains(activityGroupId.Value))
                {
                    return false;
                }
            }
            else if (entityType.Is(IntranetEntityTypeEnum.Comment))
            {
                var comment = await _commentsService.GetAsync(entityId);
                var activityType = _activityTypeHelper.GetActivityType(comment.ActivityId);

                return await CanAddLikeAsync(comment.ActivityId, (IntranetEntityTypeEnum)activityType);
            }

            return true;
        }

        private ILikeable GetActivityLikes(Guid activityId)
        {
            var service = _activitiesServiceFactory.GetService<IIntranetActivityService<IIntranetActivity>>(activityId);
            return (ILikeable)service.Get(activityId);
        }
    }
}