using System;
using uIntra.CentralFeed;
using uIntra.Core.Activity;
using uIntra.Core.Extensions;
using uIntra.Core.Links;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Groups;

namespace Compent.uIntra.Core.Feed.Links
{
    public class ActivityLinkService : ICentralFeedLinkService, IGroupFeedLinkService
    {
        private readonly IActivityTypeHelper _activityTypeHelper;
        private readonly ICentralFeedLinkProvider _centralFeedLinkProvider;
        private readonly IGroupFeedLinkProvider _groupFeedLinkProvider;
        private readonly IGroupActivityService _groupActivityService;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        private Guid CurrentUserId => _intranetUserService.GetCurrentUser().Id;

        public ActivityLinkService(
            ICentralFeedLinkProvider centralFeedLinkProvider,
            IGroupFeedLinkProvider groupFeedLinkProvider,
            IGroupActivityService groupActivityService, 
            IActivityTypeHelper activityTypeHelper, 
            IActivitiesServiceFactory activitiesServiceFactory,
            IIntranetUserService<IIntranetUser> intranetUserService)
        {
            _centralFeedLinkProvider = centralFeedLinkProvider;
            _groupFeedLinkProvider = groupFeedLinkProvider;
            _groupActivityService = groupActivityService;
            _activityTypeHelper = activityTypeHelper;
            _activitiesServiceFactory = activitiesServiceFactory;
            _intranetUserService = intranetUserService;
        }

        public IActivityLinks GetLinks(Guid activityId)
        {
            var groupId = _groupActivityService.GetGroupId(activityId);

            var activity = GetActivity(activityId);
            IActivityLinks result;
            if (groupId.HasValue)
            {
                var activityModel = activity.Map<GroupActivityTransferModel>();
                result = _groupFeedLinkProvider.GetLinks(activityModel);
            }
            else
            {
                var activityModel = activity.Map<ActivityTransferModel>();
                result = _centralFeedLinkProvider.GetLinks(activityModel);
            }
            return result;
        }

        public IActivityCreateLinks GetCreateLinks(IIntranetType activityType, Guid groupId)
        {
            var activityModel = GetActivityGroupCreateModel(activityType, groupId);
            return _groupFeedLinkProvider.GetCreateLinks(activityModel);
        }

        public IActivityCreateLinks GetCreateLinks(IIntranetType activityType)
        {
            var activityModel = GetActivityCreateModel(activityType);
            return _centralFeedLinkProvider.GetCreateLinks(activityModel);
        }

        private GroupActivityTransferCreateModel GetActivityGroupCreateModel(IIntranetType activityType, Guid groupId)
        {
            return new GroupActivityTransferCreateModel()
            {
                GroupId = groupId,
                Type = activityType,
                OwnerId = CurrentUserId
            };           
        }

        private ActivityTransferCreateModel GetActivityCreateModel(IIntranetType activityType)
        {
            return new ActivityTransferCreateModel()
            {
                Type = activityType,
                OwnerId = CurrentUserId
            };
        }

        private IIntranetActivity GetActivity(Guid id)
        {
            var activityType = _activityTypeHelper.GetActivityType(id);
            var service = GetActivityService(activityType);
            return service.Get(id);
        }

        private IIntranetActivityService<IIntranetActivity> GetActivityService(IIntranetType activityType)
        {
            return _activitiesServiceFactory.GetService<IIntranetActivityService<IIntranetActivity>>(activityType.Id);
        }
    }
}