using AutoMapper;
using Compent.Uintra.Core.Activity.Models;
using Uintra.Core.Activity;
using Uintra.Core.Location;
using Uintra.Core.Location.Entities;

namespace Compent.Uintra.Core.Activity
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