using AutoMapper;

namespace uIntra.Comments
{
    public class CommentAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Comment, CommentModel>();

            Mapper.CreateMap<CommentModel, CommentViewModel>()
                .ForMember(dst => dst.LinkPreview, o => o.Ignore())
                .ForMember(dst => dst.CanEdit, o => o.Ignore())
                .ForMember(dst => dst.CreatorProfileUrl, o => o.Ignore())
                .ForMember(dst => dst.CanDelete, o => o.Ignore())
                .ForMember(dst => dst.ModifyDate, o => o.Ignore())
                .ForMember(dst => dst.Creator, o => o.Ignore())
                .ForMember(dst => dst.ElementOverviewId, o => o.Ignore())
                .ForMember(dst => dst.CommentViewId, o => o.Ignore())
                .ForMember(dst => dst.Replies, o => o.Ignore())
                .ForMember(dst => dst.IsReply, o => o.MapFrom(el => el.ParentId.HasValue));
        }
    }
}