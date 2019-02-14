using AutoMapper;
using Compent.Uintra.Core.Activity;
using Compent.Uintra.Core.Search.SearchAutoMapperProfile;
using Uintra.Bulletins;
using Uintra.CentralFeed;
using Uintra.Comments;
using Uintra.Core.Controls.LightboxGallery;
using Uintra.Core.Jobs.AutoMapperProfiles;
using Uintra.Core.Location;
using Uintra.Core.LinkPreview;
using Uintra.Core.PagePromotion;
using Uintra.Core.Permissions.Models;
using Uintra.Events;
using Uintra.Groups;
using Uintra.Navigation;
using Uintra.News;
using Uintra.Notification;
using Uintra.Panels.Core.AutoMapperProfiles;
using Uintra.Search;
using Uintra.Tagging;
using Uintra.Subscribe;
using Uintra.Users;
using Umbraco.Core.Models.Identity;
using Umbraco.Core.Models.Membership;

namespace Compent.Uintra
{
    public static class MapperConfig
    {
        public static void RegisterMappings()
        {           
            Mapper.AddProfile<PermissionsAutoMapperProfile>();
            Mapper.AddProfile<CommentAutoMapperProfile>();
            Mapper.AddProfile<NewsAutoMapperProfile>();
            Mapper.AddProfile<Core.News.NewsAutoMapperProfile>();
            Mapper.AddProfile<LightboxAutoMapperProfile>();
            Mapper.AddProfile<Core.Navigation.NavigationAutoMapperProfile>();
            Mapper.AddProfile<NavigationAutoMapperProfile>();
            Mapper.AddProfile<EventsAutoMapperProfile>();
            Mapper.AddProfile<Core.Events.EventsAutoMapperProfile>();
            Mapper.AddProfile<BulletinsAutoMapperProfile>();
            Mapper.AddProfile<Core.Bulletins.BulletinsAutoMapperProfile>();
            Mapper.AddProfile<NotificationAutoMapperProfile>();
            Mapper.AddProfile<Core.Notification.NotificationAutoMapperProfile>();
            Mapper.AddProfile<CentralFeedAutoMapperProfile>();
            Mapper.AddProfile<IntranetUserAutoMapperProfile>();
            Mapper.AddProfile<Core.Users.IntranetMemberAutoMapperProfile>();
            Mapper.AddProfile<SearchResultAutoMapperProfile>();
            Mapper.AddProfile<SearchableActivityAutoMapperProfile>();
            Mapper.AddProfile<GroupsAutoMapperProfile>();
            Mapper.AddProfile<ActivityAutoMapperProfile>();
            Mapper.AddProfile<PagePromotionAutoMapperProfile>();
            Mapper.AddProfile<Core.PagePromotion.PagePromotionAutoMapperProfile>();
            Mapper.AddProfile<SearchAutoMapperProfile>();
            Mapper.AddProfile<JobAutoMapperProfile>();
            Mapper.AddProfile<UserTagsAutoMapperProfile>();
            Mapper.AddProfile<SubscribeAutoMapperProfiles>();
            Mapper.AddProfile<SubscribeSettingAutoMapperProfiles>();
            Mapper.AddProfile<LocationAutoMapperProfile>();
            Mapper.AddProfile<LinkPreviewAutoMapperProfile>();
            Mapper.AddProfile<TablePanelAutoMapperProfiles>();            

            var typeMaps = Mapper.GetAllTypeMaps();

            foreach (var typeMap in typeMaps)
            {
                // (╯°□°）╯︵ ┻━┻ Skip invalid umbraco map. 
                if (typeMap.SourceType == typeof(IUser) && typeMap.DestinationType == typeof(BackOfficeIdentityUser)) continue;

                Mapper.AssertConfigurationIsValid(typeMap);
            }
        }
    }
}