using AutoMapper;
using Uintra20.Core.Jobs.Configuration;

namespace Uintra20.Core.Jobs.AutoMapperProfiles
{
	public class JobAutoMapperProfile : Profile
	{
		public JobAutoMapperProfile()
		{
			CreateMap<JobSettings, JobConfiguration>()
				.ForMember(d => d.RunType, o => o.MapFrom(s => s.RunType))
				.ForMember(d => d.TimeType, o => o.MapFrom(s => s.TimeType))
				.ForMember(d => d.Enabled, o => o.MapFrom(s => s.Enabled))
				.ForMember(d => d.Time, o => o.MapFrom(s => s.Time))
				.ForMember(d => d.Date, o => o.MapFrom(s => s.Date));
		}
	}
}
