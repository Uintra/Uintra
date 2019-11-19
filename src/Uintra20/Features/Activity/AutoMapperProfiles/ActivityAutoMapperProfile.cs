using AutoMapper;
using Uintra20.Features.Activity.Models.Headers;
using Uintra20.Features.Location.Models;
using Uintra20.Features.Location.Sql;

namespace Uintra20.Features.Activity.AutoMapperProfiles
{
	public class ActivityAutoMapperProfile : Profile
	{
		public ActivityAutoMapperProfile()
		{
			CreateMap<IntranetActivityItemHeaderViewModel, ExtendedItemHeaderViewModel>()
				.ForMember(d => d.GroupInfo, o => o.Ignore());

			CreateMap<ActivityLocationEntity, ActivityLocation>();

		}
	}
}