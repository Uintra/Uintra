using AutoMapper;

namespace uIntra.Subscribe.AutoMapperProfiles
{
    public class SubscribeAutoMapperProfiles : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Subscribe, SubscribeItemModel>();
        }
    }
}