using AutoMapper;
using uIntra.Bulletins;
using uIntra.CentralFeed;
using uIntra.Core.Activity;
using uIntra.Core.Extensions;
using uIntra.Groups;
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

            Mapper.CreateMap<BulletinCreateModel, BulletinExtendedCreateModel>()
                .ForMember(dst => dst.GroupId, o => o.Ignore())
                .ForMember(dst => dst.TagIdsData, o => o.MapFrom(el => string.Empty));

            Mapper.CreateMap<BulletinEditModel, BulletinExtendedEditModel>()
                .ForMember(dst => dst.TagIdsData, o => o.MapFrom(el => string.Empty));



            Mapper.CreateMap<Bulletin, IntranetActivityItemHeaderViewModel>()
                 .IncludeBase<BulletinBase, IntranetActivityItemHeaderViewModel>();

            Mapper.CreateMap<Bulletin, SearchableActivity>()
                .IncludeBase<IntranetActivity, SearchableActivity>()
                .ForMember(dst => dst.StartDate, o => o.Ignore())
                .ForMember(dst => dst.EndDate, o => o.Ignore())
                .ForMember(dst => dst.Url, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(d => d.PublishedDate, o => o.MapFrom(s => s.PublishDate))
                .AfterMap((src, dst) =>
                {
                    var description = src.Description?.StripHtml();
                    dst.Title = description.TrimByWordEnd(50);
                });

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