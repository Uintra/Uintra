using AutoMapper;
using uIntra.Core.Jobs.Configuration;

namespace uIntra.Core.Jobs.AutoMapperProfiles
{
    public class JobAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<JobSettings, JobConfiguration>()
                .ForMember(d => d.RunType, o => o.MapFrom(s => s.RunType))
                .ForMember(d => d.TimeType, o => o.MapFrom(s => s.TimeType))
                .ForMember(d => d.Enabled, o => o.MapFrom(s => s.Enabled))
                .ForMember(d => d.Time, o => o.MapFrom(s => s.Time))
                .ForMember(d => d.Date, o => o.MapFrom(s => s.Date));
        }
    }
}
