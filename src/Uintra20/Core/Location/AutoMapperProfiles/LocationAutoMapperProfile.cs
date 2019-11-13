using AutoMapper;

namespace Uintra20.Core.Location
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