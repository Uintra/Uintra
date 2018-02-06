using AutoMapper;

namespace uIntra.Core.Location
{
    public class LocationAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<ActivityLocationEditModel, ActivityLocation>();
            Mapper.CreateMap<ActivityLocation, ActivityLocationEditModel>();
        }
    }
}
