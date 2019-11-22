using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Compent.CommandBus;
using Uintra20.Core.Member;
using Uintra20.Features.Bulletins;
using Uintra20.Features.Comments.Services;
using Uintra20.Features.Comments.Sql;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Groups.Sql;
using Uintra20.Features.Likes.Services;
using Uintra20.Features.LinkPreview;
using Uintra20.Features.Links;
using Uintra20.Features.Media;
using Uintra20.Features.Notification;
using Uintra20.Features.Notification.Services;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Caching;
using Uintra20.Infrastructure.TypeProviders;
using Uintra20.Persistence.Sql;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace Uintra20.Controllers
{
    [RoutePrefix("test")]
    public class TestController : UmbracoApiController
    {
        private readonly IBulletinsService<BulletinBase> _bulletinsService;
        private readonly IIntranetMemberService<IIntranetMember> _memberService;
        private readonly IActivityTypeProvider _activityTypeProvider;

        public TestController(IMediaService mediaService,
            IMemberService memberService2,
            UmbracoContext umbracoContext,
            UmbracoHelper umbracoHelper,
            ICacheService cacheService,
            ISqlRepository<GroupMember> groupMemberRepository,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IIntranetMemberGroupService intranetMemberGroupService,
            ICommandPublisher commandPublisher,ISqlRepository<Guid, Comment> _commentsRepository,
        ICommentLinkPreviewService _commentLinkPreviewService,
            ICommentsService _commentsService,
        ILikesService _likesService,
                              INotificationsService _notificationService,
                              IMediaHelper _mediaHelper,
                              IIntranetMediaService _intranetMediaService,
                              IGroupActivityService _groupActivityService,
                              IActivityLinkService _linkService,
                              IUserTagService _userTagService,
                              IActivityLinkPreviewService _activityLinkPreviewService,
                              IGroupService _groupService,
                              INotifierDataBuilder _notifierDataBuilder,
            
            
            IBulletinsService<BulletinBase> bulletinsService,
            IIntranetMemberService<IIntranetMember> memberService,
            IActivityTypeProvider activityTypeProvider)
        {
            
        }

        [Route("")]
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}