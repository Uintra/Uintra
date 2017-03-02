using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uCommunity.Core.App_Plugins.Core.User;
using uCommunity.Likes.App_Plugins.Likes;
using uCommunity.Likes.App_Plugins.Likes.Models;
using Umbraco.Web.Mvc;
using uCommunity.Core.App_Plugins.Core.Activity.Entities;
using uCommunity.Core.App_Plugins.Core.Persistence.Sql;
using uCommunity.Likes.App_Plugins.Likes;
using ServiceStack.OrmLite;
using uCommunity.Likes.App_Plugins.Likes.Sql;

namespace uCommunity.Likes.Controllers
{
    public class LikesController : SurfaceController
    {
        private readonly ILikesService _likesService;
        private readonly IIntranetUserService _intranetUserService;

        public LikesController()
        {
            var dbFactory = new OrmLiteConnectionFactory(@"server=192.168.0.208\SQL2014;database=TD_Intranet;user id=sa;password='q1w2e3r4'", SqlServerDialect.Provider);

            _likesService = new LikesService(new SqlRepository<Like>(dbFactory));
        }

        public LikesController(ILikesService likesService)//, IIntranetUserService intranetUserService)
        {
            _likesService = likesService;
            //_intranetUserService = intranetUserService;
        }

        public PartialViewResult Likes(Guid activityId)
        {
            var actovoty = GetLikesModel(activityId);
            return PartialView("~/App_Plugins/Likes/View/LikesView.cshtml", GetLikesModel(activityId));
        }

        [HttpPost, AllowAnonymous]
        public PartialViewResult AddLike(Guid activityId)
        {
            _likesService.Add(GetCurrentUserId(), activityId);

            return PartialView("~/App_Plugins/Likes/View/LikesView.cshtml", GetLikesModel(activityId));
        }

        [HttpPost]
        public PartialViewResult RemoveLike(Guid activityId)
        {
            _likesService.Remove(GetCurrentUserId(), activityId);

            return PartialView("~/App_Plugins/Likes/View/LikesView.cshtml", GetLikesModel(activityId));
        }

        private ActivityLikesViewModel GetLikesModel(Guid activityId)
        {
            var currentUserId = GetCurrentUserId();
            var likes = _likesService.Get(activityId).OrderByDescending(like => like.CreatedDate).ToList();
            /*var userNames = likes.Count > 0
                ? _intranetUserService.GetFullNamesByIds(likes.Select(el => el.UserId))
                : Enumerable.Empty<string>();*/
            var userNames = new List<string>();
            userNames.Add("User 1");
            userNames.Add("user 2");
            var canAddLike = _likesService.CanAdd(currentUserId, activityId);

            var likesViewModel = new ActivityLikesViewModel()
            {
                ActivityId = activityId,
                UserId = currentUserId,
                Count = likes.Count,
                CanAddLike = canAddLike,
                CanRemoveLike = !canAddLike,
                Users = userNames
            };

            return likesViewModel;
        }

        private Guid GetCurrentUserId()
        {
            return new Guid("f3ca93fc-b7ae-4cb3-a138-003e8725855e");
            //return _intranetUserService.GetCurrentUserId();
        }
    }
}