using AutoMapper;
using uCommunity.Comments.App_Plugins.Comments.Models;
using uCommunity.Comments.App_Plugins.Comments.Sql;

namespace uCommunity.Comments.App_Plugins.Comments.AutoMapperProfiles
{
    public class CommentAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Comment, CommentViewModel>()
                .ForMember(dst => dst.CreatorFullName, o => o.Ignore())
                .ForMember(dst => dst.CanEdit, o => o.Ignore())
                .ForMember(dst => dst.CanDelete, o => o.Ignore())
                .ForMember(dst => dst.ModifyDate, o => o.Ignore())
                .ForMember(dst => dst.Photo, o => o.Ignore())
                .ForMember(dst => dst.ElementOverviewId, o => o.Ignore())
                .ForMember(dst => dst.Replies, o => o.Ignore())
                .ForMember(dst => dst.IsReply, o => o.MapFrom(el => el.ParentId.HasValue));
        }
    }
}