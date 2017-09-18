using System;
using uIntra.CentralFeed;
using uIntra.Core.Activity;
using uIntra.Core.Links;
using uIntra.Core.TypeProviders;
using uIntra.Groups;

namespace Compent.uIntra.Core.Links
{
    class ActivityLinkService : ICentralFeedLinkService, IGroupFeedLinkService
    {
        private readonly IActivityTypeHelper _activityTypeHelper;
        private readonly ICentralFeedLinksProvider _centralFeedLinksProvider;
        private readonly IGroupFeedLinksProvider _groupFeedLinksProvider;
        private readonly IGroupActivityService _groupActivityService;

        public ActivityLinkService(
            ICentralFeedLinksProvider centralFeedLinksProvider,
            IGroupFeedLinksProvider groupFeedLinksProvider,
            IGroupActivityService groupActivityService, IActivityTypeHelper activityTypeHelper)
        {
            _centralFeedLinksProvider = centralFeedLinksProvider;
            _groupFeedLinksProvider = groupFeedLinksProvider;
            _groupActivityService = groupActivityService;
            _activityTypeHelper = activityTypeHelper;
        }

        public ActivityLinks GetLinks(Guid activityId)
        {
            var activityType = _activityTypeHelper.GetActivityType(activityId);
            var groupId = _groupActivityService.GetGroupId(activityId);
        }

        public ActivityCreateLinks GetCreateLinks(IIntranetType activityType, Guid groupId)
        {
            throw new NotImplementedException();
        }

        public ActivityCreateLinks GetCreateLinks(IIntranetType activityType)
        {
            throw new NotImplementedException();
        }
    }
}