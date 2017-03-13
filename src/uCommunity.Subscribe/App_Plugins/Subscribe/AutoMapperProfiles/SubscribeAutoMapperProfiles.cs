using AutoMapper;
using uCommunity.Subscribe.Model;

namespace uCommunity.Subscribe
{
    public class SubscribeAutoMapperProfiles : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Subscribe, SubscribeItemModel>();
        }
    }
}