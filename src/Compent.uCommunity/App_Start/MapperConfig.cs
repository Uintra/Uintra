using AutoMapper;
using uCommunity.Comments;
using uCommunity.Comments.AutoMapperProfiles;
using uCommunity.Core.Controls.LightboxGallery;
using uCommunity.Navigation.AutoMapperProfiles;
using uCommunity.News;

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
        }
    }
}