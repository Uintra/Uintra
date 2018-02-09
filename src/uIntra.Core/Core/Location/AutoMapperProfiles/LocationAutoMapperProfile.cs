using AutoMapper;

namespace Uintra.Core.Location
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
