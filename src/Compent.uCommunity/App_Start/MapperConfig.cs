using AutoMapper;
using uCommunity.Comments.AutoMapperProfiles;
using uCommunity.Core.Controls.LightboxGalery;
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
            Mapper.AddProfile<LightboxAutoMapperProfile>();
            Mapper.AddProfile<NavigationAutoMapperProfile>();
        }
    }
}