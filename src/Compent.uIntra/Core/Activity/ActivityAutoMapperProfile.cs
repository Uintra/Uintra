using AutoMapper;
using Compent.uIntra.Core.Activity.Models;
using uIntra.Core.Activity;

namespace Compent.uIntra.Core.Activity
{
    public class ActivityAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<IntranetActivityItemHeaderViewModel, ExtendedItemHeaderViewModel>()
                .ForMember(d => d.GroupInfo, o => o.Ignore());
        }
    }
}