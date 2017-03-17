using AutoMapper;
using uCommunity.Comments;
using uCommunity.News;

namespace Compent.uCommunity.App_Start
{
    public static class MapperConfig
    {
        public static void RegisterMappings()
        {
           Mapper.AddProfile<CommentAutoMapperProfile>();
           Mapper.AddProfile<NewsAutoMapperProfile>();
        }
    }
}