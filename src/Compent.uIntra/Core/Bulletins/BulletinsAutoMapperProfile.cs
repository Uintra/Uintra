using AutoMapper;
using uIntra.Bulletins;
using uIntra.CentralFeed;
using uIntra.Core.Activity;
using uIntra.Groups;

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

            Mapper.CreateMap<BulletinCreateModel, BulletinExtendedCreateModel>()
                .ForMember(dst => dst.GroupId, o => o.Ignore())
                .ForMember(dst => dst.TagIdsData, o => o.MapFrom(el => string.Empty));

            Mapper.CreateMap<BulletinEditModel, BulletinExtendedEditModel>()
                .ForMember(dst => dst.TagIdsData, o => o.MapFrom(el => string.Empty));

            Mapper.CreateMap<Bulletin, IntranetActivityItemHeaderViewModel>()
                 .IncludeBase<BulletinBase, IntranetActivityItemHeaderViewModel>();

            Mapper.CreateMap<Bulletin, ActivityTransferCreateModel>();

            Mapper.CreateMap<Bulletin, ActivityTransferModel>()
                .IncludeBase<Bulletin, ActivityTransferCreateModel>();

            Mapper.CreateMap<Bulletin, GroupActivityTransferCreateModel>()
                .IncludeBase<Bulletin, ActivityTransferCreateModel>();

            Mapper.CreateMap<Bulletin, GroupActivityTransferModel>()
                .IncludeBase<Bulletin, GroupActivityTransferCreateModel>();
        }
    }
}