using AutoMapper;
using Compent.uIntra.Core.Activity;
using uIntra.Bulletins;
using uIntra.CentralFeed;
using uIntra.Comments;
using uIntra.Core.Controls.LightboxGallery;
using uIntra.Events;
using uIntra.Groups;
using uIntra.Navigation;
using uIntra.News;
using uIntra.Notification;
using uIntra.Search;
using uIntra.Users;
using Umbraco.Core.Models.Identity;
using Umbraco.Core.Models.Membership;

namespace Compent.uIntra
{
    public static class MapperConfig
    {
        public static void RegisterMappings()
        {
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
            Mapper.AddProfile<Core.Users.IntranetUserAutoMapperProfile>();
            Mapper.AddProfile<SearchResultAutoMapperProfile>();
            Mapper.AddProfile<SearchableActivityAutoMapperProfile>();
            Mapper.AddProfile<GroupsAutoMapperProfile>();
            Mapper.AddProfile<ActivityAutoMapperProfile>();

            var typemaps = Mapper.GetAllTypeMaps();

            foreach (var typemap in typemaps)
            {
                // (╯°□°）╯︵ ┻━┻ Skip invalid umbraco map. 
                if (typemap.SourceType == typeof(IUser) && typemap.DestinationType == typeof(BackOfficeIdentityUser)) continue;

                Mapper.AssertConfigurationIsValid(typemap);
            }
        }
    }
}