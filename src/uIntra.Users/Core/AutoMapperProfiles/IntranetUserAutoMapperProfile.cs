using AutoMapper;
using Uintra.Core.User;
using Uintra.Core.User.DTO;

namespace Uintra.Users
{
    public class IntranetUserAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<ProfileEditModel, UpdateUserDto>()
                .ForMember(dst => dst.DeleteMedia, o => o.Ignore())
                .ForMember(dst => dst.NewMedia, o => o.Ignore());

            base.Configure();
        }
    }
}