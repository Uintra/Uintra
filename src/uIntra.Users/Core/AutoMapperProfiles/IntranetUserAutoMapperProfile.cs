using AutoMapper;
using Uintra.Core.User;
using Uintra.Users.Commands;

namespace Uintra.Users
{
    public class IntranetUserAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<ProfileEditModel, IntranetUserDTO>()
                .ForMember(dst => dst.DeleteMedia, o => o.Ignore())
                .ForMember(dst => dst.NewMedia, o => o.Ignore());

            Mapper.CreateMap<IIntranetUser, MentionUserModel>()
                .ForMember(dst => dst.Id, o => o.MapFrom(u => u.Id))
                .ForMember(dst => dst.Value, o => o.MapFrom(u => u.DisplayedName))
                .ForMember(dst => dst.Url, o => o.Ignore());


            Mapper.CreateMap<MentionModel, MentionCommand>();                

            base.Configure();
        }
    }
}