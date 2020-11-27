using AutoMapper;
using AutoMapper.Configuration;
using EmailWorker.Data.Features.EmailWorker;
using EmailWorker.Web.Infrastructure.Extensions;
using Uintra.Core.Activity.AutoMapperProfiles;
using Uintra.Core.Controls.LightboxGallery;
using Uintra.Core.Jobs.AutoMapperProfiles;
using Uintra.Core.Member.AutoMapperProfiles;
using Uintra.Core.Member.Profile.Edit.Mappers;
using Uintra.Core.Search;
using Uintra.Features.CentralFeed.AutoMapperProfiles;
using Uintra.Features.Comments.AutoMapperProfiles;
using Uintra.Features.Events.AutoMapperProfiles;
using Uintra.Features.Groups.AutoMapperProfiles;
using Uintra.Features.LinkPreview.AutoMapperProfiles;
using Uintra.Features.Location.AutoMapperProfiles;
using Uintra.Features.Navigation.AutoMapperProfiles;
using Uintra.Features.News.AutoMapperPrfiles;
using Uintra.Features.Notification.AutoMapperProfiles;
using Uintra.Features.Permissions.AutoMapperProfiles;
using Uintra.Features.Search.AutoMapperProfile;
using Uintra.Features.Social.AutoMapperProfiles;
using Uintra.Features.Subscribe.AutoMapperProfiles;
using Uintra.Features.Tagging.AutoMapperProfiles;
using Umbraco.Core.Composing;

namespace Uintra
{
    public static class MapperConfig 
    {
        public static void RegisterMappings(Composition composition)
        {
            var cfg = new MapperConfigurationExpression();
            
            cfg.AddProfile<PermissionsAutoMapperProfile>();
            cfg.AddProfile<CommentAutoMapperProfile>();
            cfg.AddProfile<LightboxAutoMapperProfile>();
            cfg.AddProfile<NavigationAutoMapperProfile>();
            cfg.AddProfile<EventsAutoMapperProfile>();
            cfg.AddProfile<SocialAutoMapperProfile>();
            cfg.AddProfile<NotificationAutoMapperProfile>();
            cfg.AddProfile<CentralFeedAutoMapperProfile>();
            cfg.AddProfile<IntranetUserAutoMapperProfile>();
            cfg.AddProfile<IntranetMemberAutoMapperProfile>();
            cfg.AddProfile<GroupsAutoMapperProfile>();
            cfg.AddProfile<ActivityAutoMapperProfile>();
            cfg.AddProfile<UserTagsAutoMapperProfile>();
            cfg.AddProfile<SubscribeAutoMapperProfiles>();
            cfg.AddProfile<SubscribeSettingAutoMapperProfiles>();
            cfg.AddProfile<LocationAutoMapperProfile>();
            cfg.AddProfile<LinkPreviewAutoMapperProfile>();
            cfg.AddProfile<MailProfile>();
            cfg.AddProfile<NewsAutoMapperProfile>();
            cfg.AddProfile<ProfileEditProfile>();
            cfg.AddProfile<JobAutoMapperProfile>();
            cfg.AddProfile<SearchAutoMapperProfile>();
            cfg.AddProfile<SearchableActivityAutoMapperProfile>();
            cfg.AddProfile<SearchResultAutoMapperProfile>();

            Mapper.Initialize(cfg);
            Mapper.AssertConfigurationIsValid();
        }
    }
}