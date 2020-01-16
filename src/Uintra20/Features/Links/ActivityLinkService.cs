using System;
using System.Threading.Tasks;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Entities;
using Uintra20.Core.Activity.Helpers;
using Uintra20.Core.Member;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.CentralFeed.Links;
using Uintra20.Features.CentralFeed.Models;
using Uintra20.Features.Groups.Links;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Links.Models;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Links
{

    //todo reimplement this due to ubaseline logic
    public class ActivityLinkService : IFeedLinkService
    {
        private readonly IActivityTypeHelper _activityTypeHelper;
        private readonly ICentralFeedLinkProvider _centralFeedLinkProvider;
        private readonly IGroupFeedLinkProvider _groupFeedLinkProvider;
        private readonly IGroupActivityService _groupActivityService;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        
        public ActivityLinkService(
            ICentralFeedLinkProvider centralFeedLinkProvider,
            IGroupFeedLinkProvider groupFeedLinkProvider,
            IGroupActivityService groupActivityService,
            IActivityTypeHelper activityTypeHelper,
            IActivitiesServiceFactory activitiesServiceFactory,
            IIntranetMemberService<IntranetMember> intranetMemberService)
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

        public async Task<IActivityLinks> GetLinksAsync(Guid activityId)
        {
            var groupId = await _groupActivityService.GetGroupIdAsync(activityId);

            //var activity = await GetActivityAsync(activityId);
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

        //public async Task<IActivityCreateLinks> GetCreateLinksAsync(Enum activityType, Guid groupId)
        //{
        //    var activityModel = await GetActivityGroupCreateModelAsync(activityType, groupId);
        //    return _groupFeedLinkProvider.GetCreateLinks(activityModel);
        //}

        //public async Task<IActivityCreateLinks> GetCreateLinksAsync(Enum activityType)
        //{
        //    var activityModel = await GetActivityCreateModelAsync(activityType);
        //    return _centralFeedLinkProvider.GetCreateLinks(activityModel);
        //}

        private GroupActivityTransferCreateModel GetActivityGroupCreateModel(Enum activityType, Guid groupId)
        {
            return new GroupActivityTransferCreateModel()
            {
                GroupId = groupId,
                Type = activityType,
                OwnerId = _intranetMemberService.GetCurrentMemberId()
            };
        }

        private ActivityTransferCreateModel GetActivityCreateModel(Enum activityType)
        {
            return new ActivityTransferCreateModel()
            {
                Type = activityType,
                OwnerId = _intranetMemberService.GetCurrentMemberId()
            };
        }

        //private async Task<GroupActivityTransferCreateModel> GetActivityGroupCreateModelAsync(Enum activityType, Guid groupId)
        //{
        //    return new GroupActivityTransferCreateModel()
        //    {
        //        GroupId = groupId,
        //        Type = activityType,
        //        OwnerId = await _intranetMemberService.GetCurrentMemberIdAsync()
        //    };
        //}

        //private async Task<ActivityTransferCreateModel> GetActivityCreateModelAsync(Enum activityType)
        //{
        //    return new ActivityTransferCreateModel()
        //    {
        //        Type = activityType,
        //        OwnerId = await _intranetMemberService.GetCurrentMemberIdAsync()
        //    };
        //}

        private IIntranetActivity GetActivity(Guid id)
        {
            var activityType = _activityTypeHelper.GetActivityType(id);
            var service = GetActivityService(activityType);
            return service.Get(id);
        }

        //private async Task<IIntranetActivity> GetActivityAsync(Guid id)
        //{
        //    var activityType = _activityTypeHelper.GetActivityType(id);
        //    var service = GetActivityService(activityType);
        //    return await service.GetAsync(id);
        //}

        private IIntranetActivityService<IIntranetActivity> GetActivityService(Enum activityType)
        {
            return _activitiesServiceFactory.GetService<IIntranetActivityService<IIntranetActivity>>(activityType);
        }
    }
}