using AutoMapper;
using AutoMapper.Configuration;
using EmailWorker.Data.Features.EmailWorker;
using EmailWorker.Web.Infrastructure.Extensions;
using Uintra20.Core.Activity.AutoMapperProfiles;
using Uintra20.Core.Controls.LightboxGallery;
using Uintra20.Core.Jobs.AutoMapperProfiles;
using Uintra20.Core.Member.AutoMapperProfiles;
using Uintra20.Core.Member.Profile.Edit.Mappers;
using Uintra20.Features.CentralFeed.AutoMapperProfiles;
using Uintra20.Features.Comments.AutoMapperProfiles;
using Uintra20.Features.Events.AutoMapperProfiles;
using Uintra20.Features.Groups.AutoMapperProfiles;
using Uintra20.Features.LinkPreview.AutoMapperProfiles;
using Uintra20.Features.Location.AutoMapperProfiles;
using Uintra20.Features.Navigation.AutoMapperProfiles;
using Uintra20.Features.News.AutoMapperPrfiles;
using Uintra20.Features.Notification.AutoMapperProfiles;
using Uintra20.Features.Permissions.AutoMapperProfiles;
using Uintra20.Features.Search.AutoMapperProfile;
using Uintra20.Features.Social.AutoMapperProfiles;
using Uintra20.Features.Subscribe.AutoMapperProfiles;
using Uintra20.Features.Tagging.AutoMapperProfiles;
using Umbraco.Core.Composing;

namespace Uintra20
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