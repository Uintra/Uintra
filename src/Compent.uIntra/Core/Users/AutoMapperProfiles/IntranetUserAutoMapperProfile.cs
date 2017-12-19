using AutoMapper;
using uIntra.Core.User;
using uIntra.Users;

namespace Compent.uIntra.Core.Users
{
    public class IntranetUserAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<IntranetUser, ProfileViewModel>();



            Mapper.CreateMap<IntranetUser, ProfileEditModel>()
                .ForMember(dst => dst.MediaRootId, o => o.Ignore())
                .ForMember(dst => dst.NewMedia, o => o.Ignore())
                .ForMember(dst => dst.MemberNotifierSettings, o => o.Ignore());

            Mapper.CreateMap<ProfileEditModel, ExtendedProfileEditModel>()
                .ForMember(dst => dst.TagIdsData, o => o.MapFrom(i => string.Empty));

            Mapper.CreateMap<IntranetUser, IntranetUserDTO>()
                .ForMember(dst => dst.DeleteMedia, o => o.Ignore())
                .ForMember(dst => dst.NewMedia, o => o.Ignore());

            base.Configure();
        }
    }
}