using AutoMapper;
using Uintra20.Features.Comments.Models;
using Uintra20.Features.Comments.Sql;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Comments.AutoMapperProfiles
{
    public class CommentAutoMapperProfile : Profile
    {
        public CommentAutoMapperProfile()
        {
            CreateMap<Comment, CommentModel>()
                .ForMember(dst => dst.LinkPreview, o => o.Ignore());

            CreateMap<CommentModel, CommentViewModel>()
                .ForMember(dst => dst.LinkPreview, o => o.Ignore())
                .ForMember(dst => dst.CanEdit, o => o.Ignore())
                .ForMember(dst => dst.CreatorProfileUrl, o => o.Ignore())
                .ForMember(dst => dst.CanDelete, o => o.Ignore())
                .ForMember(dst => dst.ModifyDate, o => o.Ignore())
                .ForMember(dst => dst.Creator, o => o.Ignore())
                .ForMember(dst => dst.Replies, o => o.Ignore())
                .ForMember(dst => dst.LikeModel, o => o.Ignore())
                .ForMember(dst => dst.CreatedDate, o => o.MapFrom(src => src.CreatedDate.ToDateTimeFormat()))
                .ForMember(dst => dst.IsReply, o => o.MapFrom(el => !el.ParentId.HasValue))
                .ForMember(dst => dst.Likes, o => o.Ignore())
                .ForMember(dst=>dst.LikedByCurrentUser, o=> o.Ignore());
        }
    }
}