using AutoMapper;
using Uintra.Core.Activity.Models.Headers;
using Uintra.Features.Location.Models;
using Uintra.Features.Location.Sql;

namespace Uintra.Core.Activity.AutoMapperProfiles
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