using AutoMapper;
using Uintra20.Core.Location;
using Uintra20.Core.Location.Sql;

namespace Uintra20.Core.Activity
{
    public class ActivityAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<IntranetActivityItemHeaderViewModel, ExtendedItemHeaderViewModel>()
                .ForMember(d => d.GroupInfo, o => o.Ignore());

            Mapper.CreateMap<ActivityLocationEntity, ActivityLocation>();

        }
    }
}