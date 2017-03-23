using AutoMapper;

namespace uCommunity.Subscribe.AutoMapperProfiles
{
    public class SubscribeAutoMapperProfiles : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Subscribe, SubscribeItemModel>();
        }
    }
}