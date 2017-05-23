using AutoMapper;
using uCommunity.CentralFeed.AutoMapperProfiles;
using uCommunity.Comments.AutoMapperProfiles;
using uCommunity.Core.Controls.LightboxGallery;
using uCommunity.Events;
using uCommunity.Navigation.AutoMapperProfiles;
using uCommunity.News;
using uCommunity.Notification.Core.Profiles;
using uCommunity.Tagging;
using Umbraco.Core.Models.Identity;
using Umbraco.Core.Models.Membership;

namespace Compent.uCommunity.App_Start
{
    public static class MapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.AddProfile<CommentAutoMapperProfile>();
            Mapper.AddProfile<NewsAutoMapperProfile>();
            Mapper.AddProfile<Core.News.NewsAutoMapperProfile>();
            Mapper.AddProfile<LightboxAutoMapperProfile>();
            Mapper.AddProfile<NavigationAutoMapperProfile>();
            Mapper.AddProfile<Core.Navigation.NavigationAutoMapperProfile>();
            Mapper.AddProfile<EventsAutoMapperProfile>();
            Mapper.AddProfile<Core.Events.EventsAutoMapperProfile>();
            Mapper.AddProfile<NotificationAutoMapperProfile>();
            Mapper.AddProfile<CentralFeedAutoMapperProfile>();
            Mapper.AddProfile<TagAutoMapperProfile>();

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