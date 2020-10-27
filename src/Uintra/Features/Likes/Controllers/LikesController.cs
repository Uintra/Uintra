using Compent.CommandBus;
using Localization.Umbraco.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using UBaseline.Core.Controllers;
using Uintra.Attributes;
using Uintra.Core;
using Uintra.Core.Activity;
using Uintra.Core.Activity.Entities;
using Uintra.Core.Activity.Helpers;
using Uintra.Core.Member.Entities;
using Uintra.Core.Member.Services;
using Uintra.Features.Comments.Services;
using Uintra.Features.Groups.Services;
using Uintra.Features.Likes.CommandBus.Commands;
using Uintra.Features.Likes.Models;
using Uintra.Features.Likes.Services;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Features.Likes.Controllers
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
        private readonly IGroupService _groupService;

        public LikesController(
            IActivitiesServiceFactory activitiesServiceFactory,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            ILikesService likesService,
            ICommandPublisher commandPublisher,
            IGroupActivityService groupActivityService,
            ICommentsService commentsService,
            IActivityTypeHelper activityTypeHelper,
            IGroupService groupService)
        {
            _activitiesServiceFactory = activitiesServiceFactory;
            _intranetMemberService = intranetMemberService;
            _likesService = likesService;
            _commandPublisher = commandPublisher;
            _groupActivityService = groupActivityService;
            _commentsService = commentsService;
            _activityTypeHelper = activityTypeHelper;
            _groupService = groupService;
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
            if (!await CanAddRemoveLikeAsync(entityId, entityType))
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
            if (await CanAddRemoveLikeAsync(entityId, entityType))
            {
                var command = new RemoveLikeCommand(entityId, entityType, await _intranetMemberService.GetCurrentMemberIdAsync());
                _commandPublisher.Publish(command);
            }

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

        private async Task<bool> CanAddRemoveLikeAsync(Guid entityId, IntranetEntityTypeEnum entityType)
        {
            if (entityType.Is(IntranetEntityTypeEnum.Social, IntranetEntityTypeEnum.News, IntranetEntityTypeEnum.Events))
            {
                var member = await _intranetMemberService.GetCurrentMemberAsync();
                var activityGroupId = _groupActivityService.GetGroupId(entityId);

                if (activityGroupId.HasValue)
                {
                    var group = _groupService.Get(activityGroupId.Value);
                    if (group == null || group.IsHidden)
                        return false;
                }

                if (activityGroupId.HasValue && !member.GroupIds.Contains(activityGroupId.Value))
                {
                    return false;
                }
            }
            else if (entityType.Is(IntranetEntityTypeEnum.Comment))
            {
                var comment = await _commentsService.GetAsync(entityId);
                var activityType = _activityTypeHelper.GetActivityType(comment.ActivityId);

                return await CanAddRemoveLikeAsync(comment.ActivityId, (IntranetEntityTypeEnum)activityType);
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