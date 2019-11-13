using AutoMapper;
using Uintra20.Core.Activity;
using Uintra20.Core.Bulletins;
using Uintra20.Core.CentralFeed;
using Uintra20.Core.Comments;
using Uintra20.Core.Controls.LightboxGallery;
using Uintra20.Core.Events;
using Uintra20.Core.Groups;
using Uintra20.Core.LinkPreview;
using Uintra20.Core.Location;
using Uintra20.Core.Navigation;
using Uintra20.Core.Notification;
using Uintra20.Core.Permissions;
using Uintra20.Core.Subscribe;
using Uintra20.Core.Subscribe.AutoMapperProfiles;
using Uintra20.Core.Tagging;
using Uintra20.Core.User;
using Umbraco.Core.Models.Identity;
using Umbraco.Core.Models.Membership;

namespace Uintra20.App_Start
{
    public static class MapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.AddProfile<PermissionsAutoMapperProfile>();
            Mapper.AddProfile<CommentAutoMapperProfile>();
            //Mapper.AddProfile<NewsAutoMapperProfile>();
            //Mapper.AddProfile<Core.News.NewsAutoMapperProfile>();
            Mapper.AddProfile<LightboxAutoMapperProfile>();
            Mapper.AddProfile<NavigationAutoMapperProfile>();
            Mapper.AddProfile<EventsAutoMapperProfile>();
            Mapper.AddProfile<BulletinsAutoMapperProfile>();
            Mapper.AddProfile<NotificationAutoMapperProfile>();
            Mapper.AddProfile<CentralFeedAutoMapperProfile>();
            Mapper.AddProfile<IntranetUserAutoMapperProfile>();
            Mapper.AddProfile<IntranetMemberAutoMapperProfile>();
            //Mapper.AddProfile<SearchResultAutoMapperProfile>();
            //Mapper.AddProfile<SearchableActivityAutoMapperProfile>();
            Mapper.AddProfile<GroupsAutoMapperProfile>();
            Mapper.AddProfile<ActivityAutoMapperProfile>();
            //Mapper.AddProfile<PagePromotionAutoMapperProfile>();
            //Mapper.AddProfile<Core.PagePromotion.PagePromotionAutoMapperProfile>();
            //Mapper.AddProfile<SearchAutoMapperProfile>();
            //Mapper.AddProfile<JobAutoMapperProfile>();
            Mapper.AddProfile<UserTagsAutoMapperProfile>();
            Mapper.AddProfile<SubscribeAutoMapperProfiles>();
            Mapper.AddProfile<SubscribeSettingAutoMapperProfiles>();
            Mapper.AddProfile<LocationAutoMapperProfile>();
            Mapper.AddProfile<LinkPreviewAutoMapperProfile>();
            //Mapper.AddProfile<TablePanelAutoMapperProfiles>();

            var typeMaps = Mapper.GetAllTypeMaps();

            foreach (var typeMap in typeMaps)
            {
                // (╯°□°）╯︵ ┻━┻ Skip invalid umbraco map. 
                if (typeMap.SourceType == typeof(IUser) &&
                    typeMap.DestinationType == typeof(BackOfficeIdentityUser)) continue;

                Mapper.AssertConfigurationIsValid(typeMap);
            }
        }
    }
}