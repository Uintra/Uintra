using AutoMapper;
using uIntra.Core.User;

namespace uIntra.Core.Activity
{
    public class ActivityAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<IIntranetUser, IntranetActivityCreatorViewModel>();
        }
    }
}