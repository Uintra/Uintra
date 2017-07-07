using AutoMapper;
using uIntra.Bulletins;
using uIntra.Core.Activity;
using uIntra.Search;

namespace Compent.uIntra.Core.Bulletins
{
    public class BulletinsAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Bulletin, BulletinExtendedViewModel>()
                .IncludeBase<BulletinBase, BulletinViewModel>()
                .ForMember(dst => dst.LikesInfo, o => o.MapFrom(el => el))
                .ForMember(dst => dst.CommentsInfo, o => o.MapFrom(el => el));

            Mapper.CreateMap<Bulletin, BulletinExtendedItemViewModel>()
                .IncludeBase<BulletinBase, BulletinItemViewModel>()
                .ForMember(dst => dst.LikesInfo, o => o.MapFrom(el => el))
                .ForMember(dst => dst.CommentsInfo, o => o.MapFrom(el => el));

            Mapper.CreateMap<Bulletin, IntranetActivityItemHeaderViewModel>()
                 .IncludeBase<BulletinBase, IntranetActivityItemHeaderViewModel>();

            Mapper.CreateMap<Bulletin, SearchableActivity>()
                .ForMember(dst => dst.StartDate, o => o.Ignore())
                .ForMember(dst => dst.EndDate, o => o.Ignore())
                .ForMember(dst => dst.Url, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(d => d.PublishedDate, o => o.MapFrom(s => s.PublishDate))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Description))
                .IncludeBase<IntranetActivity, SearchableActivity>();
        }
    }
}