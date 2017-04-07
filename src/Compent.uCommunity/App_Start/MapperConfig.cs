using AutoMapper;
using uCommunity.Comments.AutoMapperProfiles;
using uCommunity.Core.Controls.LightboxGallery;
using uCommunity.Events;
using uCommunity.Navigation.AutoMapperProfiles;
using uCommunity.News;
using uCommunity.Notification.Core.Profiles;

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
        }
    }
}