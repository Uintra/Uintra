using AutoMapper;
using Uintra20.Features.Location.Models;

namespace Uintra20.Features.Location.AutoMapperProfiles
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