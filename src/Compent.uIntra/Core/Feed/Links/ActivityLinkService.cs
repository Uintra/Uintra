using System;
using Uintra.CentralFeed;
using Uintra.Core.Activity;
using Uintra.Core.Extensions;
using Uintra.Core.Links;
using Uintra.Core.User;
using Uintra.Groups;

namespace Compent.Uintra.Core.Feed.Links
{
    public class ActivityLinkService : IFeedLinkService
    {
        private readonly IActivityTypeHelper _activityTypeHelper;
        private readonly ICentralFeedLinkProvider _centralFeedLinkProvider;
        private readonly IGroupFeedLinkProvider _groupFeedLinkProvider;
        private readonly IGroupActivityService _groupActivityService;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;

        private Guid CurrentMemberId => _intranetMemberService.GetCurrentMember().Id;

        public ActivityLinkService(
            ICentralFeedLinkProvider centralFeedLinkProvider,
            IGroupFeedLinkProvider groupFeedLinkProvider,
            IGroupActivityService groupActivityService, 
            IActivityTypeHelper activityTypeHelper, 
            IActivitiesServiceFactory activitiesServiceFactory,
            IIntranetMemberService<IIntranetMember> intranetMemberService)
        {
            _centralFeedLinkProvider = centralFeedLinkProvider;
            _groupFeedLinkProvider = groupFeedLinkProvider;
            _groupActivityService = groupActivityService;
            _activityTypeHelper = activityTypeHelper;
            _activitiesServiceFactory = activitiesServiceFactory;
            _intranetMemberService = intranetMemberService;
        }

        public IActivityLinks GetLinks(Guid activityId)
        {
            var groupId = _groupActivityService.GetGroupId(activityId);

            var activity = GetActivity(activityId);
            IActivityLinks result;
            if (groupId.HasValue)
            {
                var activityModel = activity.Map<GroupActivityTransferModel>();
                activityModel.GroupId = groupId.Value;
                result = _groupFeedLinkProvider.GetLinks(activityModel);
            }
            else
            {
                var activityModel = activity.Map<ActivityTransferModel>();
                result = _centralFeedLinkProvider.GetLinks(activityModel);
            }
            return result;
        }

        public IActivityCreateLinks GetCreateLinks(Enum activityType, Guid groupId)
        {
            var activityModel = GetActivityGroupCreateModel(activityType, groupId);
            return _groupFeedLinkProvider.GetCreateLinks(activityModel);
        }

        public IActivityCreateLinks GetCreateLinks(Enum activityType)
        {
            var activityModel = GetActivityCreateModel(activityType);
            return _centralFeedLinkProvider.GetCreateLinks(activityModel);
        }

        private GroupActivityTransferCreateModel GetActivityGroupCreateModel(Enum activityType, Guid groupId)
        {
            return new GroupActivityTransferCreateModel()
            {
                GroupId = groupId,
                Type = activityType,
                OwnerId = CurrentMemberId
            };           
        }

        private ActivityTransferCreateModel GetActivityCreateModel(Enum activityType)
        {
            return new ActivityTransferCreateModel()
            {
                Type = activityType,
                OwnerId = CurrentMemberId
            };
        }

        private IIntranetActivity GetActivity(Guid id)
        {
            var activityType = _activityTypeHelper.GetActivityType(id);
            var service = GetActivityService(activityType);
            return service.Get(id);
        }

        private IIntranetActivityService<IIntranetActivity> GetActivityService(Enum activityType)
        {
            return _activitiesServiceFactory.GetService<IIntranetActivityService<IIntranetActivity>>(activityType);
        }
    }
}