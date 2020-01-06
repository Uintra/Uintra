using AutoMapper;
using AutoMapper.Configuration;
using EmailWorker.Data.Features.EmailWorker;
using Uintra20.Core.Activity.AutoMapperProfiles;
using Uintra20.Core.Controls.LightboxGallery;
using Uintra20.Core.Member.AutoMapperProfiles;
using Uintra20.Features.Bulletins.AutoMapperProfiles;
using Uintra20.Features.CentralFeed.AutoMapperProfiles;
using Uintra20.Features.Comments.AutoMapperProfiles;
using Uintra20.Features.Events.AutoMapperProfiles;
using Uintra20.Features.Groups.AutoMapperProfiles;
using Uintra20.Features.LinkPreview.AutoMapperProfiles;
using Uintra20.Features.Location.AutoMapperProfiles;
using Uintra20.Features.Navigation.AutoMapperProfiles;
using Uintra20.Features.Notification.AutoMapperProfiles;
using Uintra20.Features.Permissions.AutoMapperProfiles;
using Uintra20.Features.Subscribe.AutoMapperProfiles;
using Uintra20.Features.Tagging.AutoMapperProfiles;
using Umbraco.Core.Composing;

namespace Uintra20
{
    public static class MapperConfig 
    {
        public static void RegisterMappings(Composition composition)
        {
            //Mapper.AddProfile<PermissionsAutoMapperProfile>();
            //Mapper.AddProfile<CommentAutoMapperProfile>();
            ////Mapper.AddProfile<NewsAutoMapperProfile>();
            ////Mapper.AddProfile<Core.News.NewsAutoMapperProfile>();
            //Mapper.AddProfile<LightboxAutoMapperProfile>();
            //Mapper.AddProfile<NavigationAutoMapperProfile>();
            //Mapper.AddProfile<EventsAutoMapperProfile>();
            //Mapper.AddProfile<SocialAutoMapperProfile>();
            //Mapper.AddProfile<NotificationAutoMapperProfile>();
            //Mapper.AddProfile<CentralFeedAutoMapperProfile>();
            //Mapper.AddProfile<IntranetUserAutoMapperProfile>();
            //Mapper.AddProfile<IntranetMemberAutoMapperProfile>();
            ////Mapper.AddProfile<SearchResultAutoMapperProfile>();
            ////Mapper.AddProfile<SearchableActivityAutoMapperProfile>();
            //Mapper.AddProfile<GroupsAutoMapperProfile>();
            //Mapper.AddProfile<ActivityAutoMapperProfile>();
            ////Mapper.AddProfile<PagePromotionAutoMapperProfile>();
            ////Mapper.AddProfile<Core.PagePromotion.PagePromotionAutoMapperProfile>();
            ////Mapper.AddProfile<SearchAutoMapperProfile>();
            ////Mapper.AddProfile<JobAutoMapperProfile>();
            //Mapper.AddProfile<UserTagsAutoMapperProfile>();
            //Mapper.AddProfile<SubscribeAutoMapperProfiles>();
            //Mapper.AddProfile<SubscribeSettingAutoMapperProfiles>();
            //Mapper.AddProfile<LocationAutoMapperProfile>();
            //Mapper.AddProfile<LinkPreviewAutoMapperProfile>();
            ////Mapper.AddProfile<TablePanelAutoMapperProfiles>();

            //var typeMaps = Mapper.GetAllTypeMaps();

            //foreach (var typeMap in typeMaps)
            //{
            //    // (╯°□°）╯︵ ┻━┻ Skip invalid umbraco map. 
            //    if (typeMap.SourceType == typeof(IUser) &&
            //        typeMap.DestinationType == typeof(BackOfficeIdentityUser)) continue;

            //    Mapper.AssertConfigurationIsValid(typeMap);
            //}

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
            
            Mapper.Initialize(cfg);
            Mapper.AssertConfigurationIsValid();
        }
    }
}