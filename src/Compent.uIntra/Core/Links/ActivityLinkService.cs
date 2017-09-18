using System;
using uIntra.CentralFeed;
using uIntra.Core.Links;
using uIntra.Core.TypeProviders;
using uIntra.Groups;

namespace Compent.uIntra.Core.Links
{
    class ActivityLinkService : ICentralFeedLinkService, IGroupFeedLinkService
    {
        public ActivityLinks GetLinks(Guid activityId)
        {
            throw new NotImplementedException();
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