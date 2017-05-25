using AutoMapper;

namespace uIntra.Subscribe
{
    public class SubscribeAutoMapperProfiles : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Subscribe, SubscribeItemModel>();
        }
    }
}