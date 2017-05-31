using AutoMapper;

namespace uIntra.Users
{
    public class IntranetUserAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<IntranetUser, ProfileViewModel>();
        }
    }
}