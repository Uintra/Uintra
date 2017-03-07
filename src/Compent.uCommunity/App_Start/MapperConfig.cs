using AutoMapper;
using uCommunity.Comments.App_Plugins.Comments.AutoMapperProfiles;

namespace Compent.uCommunity.App_Start
{
    public static class MapperConfig
    {
        public static void RegisterMappings()
        {
           Mapper.AddProfile<CommentAutoMapperProfile>();
        }
    }
}