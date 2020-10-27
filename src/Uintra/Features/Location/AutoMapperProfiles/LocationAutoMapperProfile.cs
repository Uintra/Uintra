using AutoMapper;
using Uintra.Features.Location.Models;

namespace Uintra.Features.Location.AutoMapperProfiles
{
    public class LocationAutoMapperProfile : Profile
    {
	    public LocationAutoMapperProfile()
        {
            CreateMap<ActivityLocationEditModel, ActivityLocation>();
            CreateMap<ActivityLocation, ActivityLocationEditModel>();
        }
    }
}